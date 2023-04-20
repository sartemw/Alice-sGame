using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Fish;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.Services.FishCollectorService;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private readonly IInputService _inputService;
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _persistentProgressService;
    private GameObject _heroGameObject;
    private readonly IWindowService _windowService;
    private readonly IGameStateMachine _stateMachine;
    private readonly DiContainer _diContainer;

    public GameFactory(
      IInputService inputService,
      IAssetProvider assets, 
      IStaticDataService staticData, 
      IRandomService randomService, 
      IPersistentProgressService persistentProgressService, 
      IWindowService windowService, 
      IGameStateMachine stateMachine,
      DiContainer diContainer)
    {
      _diContainer = diContainer;
      _inputService = inputService;
      _assets = assets;
      _staticData = staticData;
      _randomService = randomService;
      _persistentProgressService = persistentProgressService;
      _windowService = windowService;
      _stateMachine = stateMachine;
    }
    
    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.EnemySpawner);
      await _assets.Load<GameObject>(AssetAddress.FishSpawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      _heroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
      _heroGameObject.GetComponent<HeroMove>().Construct(_inputService);
      _heroGameObject.GetComponent<HeroAttack>().Construct(_inputService);
      
      return _heroGameObject;
    }

    public async Task CreateLevelTransfer(Vector3 at)
    {
      GameObject prefab = await InstantiateRegisteredAsync(AssetAddress.LevelTransferTrigger, at);
      LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();
      CanOpenDoor canOpenDoor = prefab.GetComponent<CanOpenDoor>();
      LevelStaticData levelStaticData = _staticData.ForLevel(SceneManager.GetActiveScene().name);

      levelTransfer.TransferTo = levelStaticData.LevelTransfer.TransferTo;
      levelTransfer.Construct(_stateMachine);
      levelTransfer.GetComponent<BoxCollider2D>().enabled = false;
      
      canOpenDoor.Construct(_diContainer.Resolve<IRepaintingService>());
    }

   public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
      
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_persistentProgressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        openWindowButton.Init(_windowService);

      return hud;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      LootPiece lootPiece = InstantiateRegistered(prefab)
        .GetComponent<LootPiece>();
      
      lootPiece.Construct(_persistentProgressService.Progress.WorldData);

      return lootPiece;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(_heroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<AgentMoveToPlayer>()?.Construct(_heroGameObject.transform, monsterData.MoveSpeed);
      monster.GetComponent<RotateToHero>()?.Construct(_heroGameObject.transform);

      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.Construct(this, _randomService);
      lootSpawner.SetLootValue(monsterData.MinLootValue, monsterData.MaxLootValue);

      return monster;
    }

    public async Task CreateEnemySpawner(string spawnerId, Vector3 at, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.EnemySpawner);
      
      EnemySpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<EnemySpawnPoint>();
      
      spawner.Construct(this);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
    }
    
    public async Task CreateFishSpawner(string spawnerId, ColorType color, FishBehaviourEnum behaviour, Vector2 at)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.FishSpawner);
      FishSpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<FishSpawnPoint>();
      
      spawner.ColorType = color;
      spawner.Id = spawnerId;
      spawner.FishBehaviour = behaviour;
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _assets.Cleanup();
    }
    
    private GameObject InstantiateRegistered(GameObject prefab, Vector2 at)
    {
      GameObject gameObject = _diContainer.InstantiatePrefab(prefab, at, Quaternion.identity, null);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }
    
    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject gameObject = _diContainer.InstantiatePrefab(prefab);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector2 at)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath, at: at);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(path: prefabPath);
      RegisterProgressWatchers(gameObject);

      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }
  }
}