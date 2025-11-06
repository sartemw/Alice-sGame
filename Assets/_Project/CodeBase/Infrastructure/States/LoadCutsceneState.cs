using System.Threading.Tasks;
using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Logic.Curtain;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
{
    public class LoadCutsceneState: IPayloadedState<string>
    {
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IPaintingService _paintService;

        public LoadCutsceneState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            LoadingCurtain curtain, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = curtain;
            _gameFactory = diContainer.Resolve<IGameFactory>();
            _progressService = diContainer.Resolve<IPersistentProgressService>();
            _staticData = diContainer.Resolve<IStaticDataService>();
            _paintService = diContainer.Resolve<IPaintingService>();
        }
        
        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private async void OnLoaded()
        {
            LevelStaticData levelData = LevelStaticData();
            
            await InitSpawners(levelData);
            InformProgressReaders();
            
            // LevelStaticData levelData = LevelStaticData();
            //
            // EventBus.Invoke(new LoadLevelSignals
            // {
            //     Music = levelData.Music
            // });

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }
        
        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.PlayerProgress);
        }
        
        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);
        
        private async Task InitSpawners(LevelStaticData levelStaticData)
        {
            foreach (EnemySpawnerStaticData spawnerData in levelStaticData.EnemySpawners)
                await _gameFactory.CreateEnemySpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeId, levelStaticData.Cutscene);
        }
    }
}