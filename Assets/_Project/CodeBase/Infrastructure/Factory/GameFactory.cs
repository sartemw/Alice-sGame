using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.CodeBase.Enemy;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Hero;
using _Project.CodeBase.Infrastructure.AssetManagement;
using _Project.CodeBase.Infrastructure.Effects;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Logic;
using _Project.CodeBase.Logic.Door;
using _Project.CodeBase.Logic.EnemySpawners;
using _Project.CodeBase.Services.Analytics;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Input;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.Randomizer;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.Services.SaveLoad;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.UI.Elements;
using _Project.CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.CodeBase.Infrastructure.Factory
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
    private readonly IAnalyticsService _analyticsService;
    private readonly IGameStateMachine _stateMachine;
    private readonly IPaintingService _paintingService;
    private readonly DiContainer _diContainer;
    private readonly ICoroutineRunner _coroutineRunner;

    private Queue<PoolInk> _poolInk = new Queue<PoolInk>();
    private bool _canCreateInk = true;

    public GameFactory(
      IInputService inputService,
      IAssetProvider assets, 
      IStaticDataService staticData, 
      IRandomService randomService, 
      IPersistentProgressService persistentProgressService, 
      IWindowService windowService, 
      IAnalyticsService analyticsService,
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
      _analyticsService = analyticsService;
      _stateMachine = stateMachine;
      _paintingService = diContainer.Resolve<IPaintingService>();
      _coroutineRunner = diContainer.Resolve<ICoroutineRunner>();
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
      HeroStaticData heroStaticData = _staticData.ForHero(HeroTypeId.Cat);
      
      HeroMove heroMove = _heroGameObject.GetComponent<HeroMove>();
      heroMove.Construct(_inputService);
      heroMove._movementSpeed = heroStaticData.MoveSpeed;

      HeroAttack heroAttack = _heroGameObject.GetComponent<HeroAttack>();
      heroAttack.Construct(_inputService);
      heroAttack.AttackDistance = heroStaticData.EffectiveDistance;

      HeroHealth heroHealth = _heroGameObject.GetComponent<HeroHealth>();
      heroHealth.Construct(_stateMachine);
      
      return _heroGameObject;
    }

    public async Task CreateLevelTransfer(Vector3 at)
    {
      GameObject prefab = await InstantiateRegisteredAsync(AssetAddress.LevelTransferTrigger, at);
      LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();
      DoorOpener doorOpener = prefab.GetComponent<DoorOpener>();
      LevelStaticData levelStaticData = _staticData.ForLevel(SceneManager.GetActiveScene().name);

      levelTransfer.TransferTo = levelStaticData.LevelTransfer.TransferTo;
      levelTransfer.Construct(_stateMachine,  _diContainer.Resolve<ISaveLoadService>());
      levelTransfer.GetComponent<BoxCollider2D>().enabled = false;

      doorOpener.Construct(_diContainer.Resolve<IPaintingService>());
    }

   public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
      
      hud.GetComponentInChildren<LootCounter>()
        .Construct(_persistentProgressService.PlayerProgress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
        openWindowButton.Init(_windowService, _analyticsService, _diContainer.Resolve<IAudioService>());

      return hud;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      LootPiece lootPiece = InstantiateRegistered(prefab)
        .GetComponent<LootPiece>();
      
      lootPiece.Construct(_persistentProgressService.PlayerProgress.WorldData);

      return lootPiece;
    }
    
    private struct PoolInk
    {
      public Vector2 StartPosition;
      public Vector2 MoveTo;
      public Paintable ColoredObj;
      public IPaintingService PaintingService;
    }

    public async Task CreateInkToBlot(Vector2 at, Vector2 to)
    {
      _poolInk.Enqueue(new PoolInk()
      {
        StartPosition = at,
        MoveTo = to,
        PaintingService = _paintingService
      });
      
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Ink);
      
      if (_canCreateInk)
        _coroutineRunner.StartCoroutine(PoolingInk(prefab));
    }

    public async Task CreateInk(Vector2 at, Vector2 moveTo, Paintable coloredObj, IPaintingService paintingService)
    {
      _poolInk.Enqueue(new PoolInk()
      {
        StartPosition = at,
        MoveTo = moveTo,
        ColoredObj = coloredObj,
        PaintingService = paintingService
      });
      
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Ink);
      
      if (_canCreateInk)
        _coroutineRunner.StartCoroutine(PoolingInk(prefab));
    }

    // public void CanCreateInk() => 
    //   _countInk = 0;

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
    
    public async Task<GameObject> CreateCutsceneMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
      
      return monster;
    }

    public async Task CreateEnemySpawner(string spawnerId, Vector3 at, MonsterTypeId monsterTypeId, bool isCutscene = false)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.EnemySpawner);
      
      EnemySpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<EnemySpawnPoint>();
      spawner.Construct(this, isCutscene);
      spawner.MonsterTypeId = monsterTypeId;
      spawner.Id = spawnerId;
    }

    public async Task CreateFishSpawner(string spawnerId, ColorType color, FishBehaviourEnum behaviour, Vector2 at)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.FishSpawner);
      FishSpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<FishSpawnPoint>();

      spawner.transform.parent = Camera.main.transform;
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


    private IEnumerator PoolingInk(GameObject prefab)
    {
      _canCreateInk = false;
      while (_poolInk.Count != 0)
      {
        PoolInk inkTemp = _poolInk.Dequeue();
        Ink ink = InstantiateRegistered(prefab, inkTemp.StartPosition)
          .GetComponent<Ink>();
        ink.Construct(inkTemp.MoveTo, inkTemp.ColoredObj, inkTemp.PaintingService, _staticData.ForConfig(), _diContainer.Resolve<IAudioService>());
        
        yield return new WaitForSeconds(0.5f);
      }

      _canCreateInk = true;
    }
  }
}