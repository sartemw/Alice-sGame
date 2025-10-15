using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.CodeBase.CameraLogic;
using _Project.CodeBase.Data;
using _Project.CodeBase.Enemy;
using _Project.CodeBase.Events;
using _Project.CodeBase.Hero;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Logic;
using _Project.CodeBase.Logic.Curtain;
using _Project.CodeBase.Services.Analytics;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.UI.Elements;
using _Project.CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;
    private readonly IAudioService _audioService;
    private readonly IAnalyticsService _analyticsService;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, DiContainer diContainer)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = diContainer.Resolve<IGameFactory>();
      _progressService = diContainer.Resolve<IPersistentProgressService>();
      _staticData = diContainer.Resolve<IStaticDataService>();
      _uiFactory = diContainer.Resolve<IUIFactory>();
      _audioService = diContainer.Resolve<IAudioService>();
      _analyticsService = diContainer.Resolve<IAnalyticsService>();
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _gameFactory.WarmUp();

      // _sceneLoader.Load("0-5", OnLoaded);
      // return;
      if (sceneName == "GameEnd")
      {
        _sceneLoader.Load(sceneName, GameEnd);
      }
      else
      {
        _sceneLoader.Load(sceneName, OnLoaded);
      }
      
      Debug.Log($"<color=yellow> Load {sceneName} scene</color>");
      _analyticsService.Send($"Load {sceneName} scene");
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private async void OnLoaded()
    {      
      await InitUIRoot();

      await InitGameWorld();
      
      InformProgressReaders();

      
      
      _stateMachine.Enter<GameLoopState>();
    }

    private void GameEnd()
    {
      _stateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot() => 
      await _uiFactory.CreateUIRoot();

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.PlayerProgress);
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      GameObject hero = await InitHero(levelData);
      await InitSpawners(levelData);
      await InitLootPieces();
      await InitLevelTransfer(levelData);
      await InitHud(hero);
      
      EventBus.Invoke(new LoadLevelSignals
      {
        Music = levelData.Music
      });

      CameraFollow(hero);
    }

    private async Task InitSpawners(LevelStaticData levelStaticData)
    {
      foreach (EnemySpawnerStaticData spawnerData in levelStaticData.EnemySpawners)
        await _gameFactory.CreateEnemySpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeId);
      
      foreach (FishSpawnerStaticData spawnerData in levelStaticData.FishSpawners)
        await _gameFactory.CreateFishSpawner(spawnerData.Id, spawnerData.FishColor, spawnerData.FishBehaviour, spawnerData.Position);
    }

    private async Task InitLootPieces()
    {
      foreach (KeyValuePair<string, LootPieceData> item in _progressService.PlayerProgress.WorldData.LootData.LootPiecesOnScene.Dictionary)
      {
        LootPiece lootPiece = await _gameFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = item.Key;
        lootPiece.Initialize(item.Value.Loot);
        lootPiece.transform.position = item.Value.Position.AsUnityVector();
      }
    }

    private async Task<GameObject> InitHero(LevelStaticData levelStaticData) => 
      await _gameFactory.CreateHero(levelStaticData.InitialHeroPosition);

    private async Task InitLevelTransfer(LevelStaticData levelData) => 
        await _gameFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _gameFactory.CreateHud();
      
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private void CameraFollow(GameObject hero)
    {
      if (Camera.main != null) 
        Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
  }
}