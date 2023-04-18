using CodeBase.Fish;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Ads;
using CodeBase.Services.FishCollectorService;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ICoroutineRunner
    {
        public GameObject FishPrefab;
        public LoadingCurtain CurtainPrefab;

        private Game _game;
        private AllServices _services;
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticData;
        private IInputService _inputService;
        private IAdsService _adsService;
        private IRandomService _randomService;
        private IPersistentProgressService _persistentProgress;
        private IUIFactory _uiFactory;
        private IWindowService _windowService;
        private IGameFactory _gameFactory;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;

        public override void InstallBindings()
        {
            BindBootstrapInstaller();
            BindAllServices();

            BindStaticDataService();
            BindAdsService();
            BindAssetProvider();
            BindInputService();
            BindRandomService();
            BindPersistentProgressService();
            BindUIFactory();
            BindWindowService();
            
            BindFishCollectorService();
            
            BindFishFactory();
            BindPoolFactory();
        }

        private void BindWindowService()
        {
            _windowService = new WindowService(_uiFactory);
            _services.RegisterSingle<IWindowService>(_windowService);
            Container
                .Bind<IWindowService>()
                .FromInstance(_windowService)
                .AsSingle();
        }

        private void BindUIFactory()
        {
            _uiFactory = new UIFactory(_assetProvider, _staticData, _persistentProgress, _adsService);
            _services.RegisterSingle<IUIFactory>(_uiFactory);
            Container
                .Bind<IUIFactory>()
                .FromInstance(_uiFactory)
                .AsSingle();
        }

        private void BindPersistentProgressService()
        {
            _persistentProgress = new PersistentProgressService();
            _services.RegisterSingle<IPersistentProgressService>(_persistentProgress);
            Container
                .Bind<IPersistentProgressService>()
                .FromInstance(_persistentProgress)
                .AsSingle();
        }

        private void BindRandomService()
        {
            _randomService = new RandomService();
            _services.RegisterSingle<IRandomService>(_randomService);
            Container
                .Bind<IRandomService>()
                .FromInstance(_randomService)
                .AsSingle();
        }

        private void BindAdsService()
        {
            _adsService = new AdsService();
            _adsService.Initialize();
            Container
                .Bind<IAdsService>()
                .FromInstance(_adsService)
                .AsSingle();
            _services.RegisterSingle<IAdsService>(_adsService);
        }
        private void BindBootstrapInstaller()
        {
            Container
                .BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindAllServices()
        {
            _services = new AllServices();

            Container
                .Bind<AllServices>()
                .FromInstance(_services)
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            _staticData = new StaticDataService();
            
            Container.Bind<IStaticDataService>()
                .FromInstance(_staticData)
                .AsSingle();
            
            _services.RegisterSingle<IStaticDataService>(_staticData);

            _staticData.Load();
        }

        private void BindAssetProvider()
        {
            _assetProvider = new AssetProvider();
            
            Container.Bind<IAssetProvider>()
                .FromInstance(_assetProvider)
                .AsSingle();
            
            _services.RegisterSingle<IAssetProvider>(_assetProvider);

            _assetProvider.Initialize();
        }

        private void BindInputService()
        {
            _inputService = ChangeInputService();
            
            Container
                .Bind<IInputService>()
                .FromInstance(_inputService)
                .AsSingle();
            _services.RegisterSingle<IInputService>(_inputService);
        }

        private void BindFishCollectorService()
        {
            Container
                .Bind<IRepaintingService>()
                .To<RepaintingService>()
                .AsSingle();
        }

        private void BindFishFactory()
        {
           // Container.BindInstance(FishPrefab).AsSingle();

            Container
                .BindFactory<ColoredFish, ColoredFish.Factory>()
                .FromComponentInNewPrefab(FishPrefab);
        }

        private void BindPoolFactory()
        {
            Container
                .Bind<IPoolFactory>()
                .To<PoolFactory>()
                .AsSingle();
        }

        private static IInputService ChangeInputService() =>
            Application.isEditor
                ? (IInputService) new StandaloneInputService()
                : new MobileInputService();

        public void Initialize()
        {
            CreateGame();
            BindGameStateMachine();
            //BindGameFactory();
            //BindSaveLoadService();
        }

        private void BindGameStateMachine()
        {
            _stateMachine = _game.StateMachine;
            Container
                .Bind<IGameStateMachine>()
                .FromInstance(_stateMachine)
                .AsSingle();
        }

        private void BindSaveLoadService()
        {
            _saveLoadService = new SaveLoadService(_persistentProgress, _gameFactory);
            _services.RegisterSingle<ISaveLoadService>(_saveLoadService);
            Container
                .Bind<ISaveLoadService>()
                .FromInstance(_saveLoadService)
                .AsSingle();
        }

        private void BindGameFactory()
        {
            _gameFactory = new GameFactory(
                _inputService,
                _assetProvider,
                _staticData,
                _randomService,
                _persistentProgress,
                _windowService,
                _stateMachine);
            
            _services.RegisterSingle<IGameFactory>(_gameFactory);

            Container
                .Bind<IGameFactory>()
                .FromInstance(_gameFactory)
                .AsSingle();
        }

        private void CreateGame()
        {
            _game = new Game(this, Instantiate(CurtainPrefab), _services);
            _game.StateMachine.Enter<BootstrapState>();
            
            Container.Bind<Game>().FromInstance(_game).AsSingle();
        }
    }
}