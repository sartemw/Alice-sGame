using CodeBase.Fish;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Mask;
using CodeBase.Services;
using CodeBase.Services.Ads;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Repainting;
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
        public LoadingCurtain CurtainPrefab;
        
        public Material Colored;
        public Material Colorless;

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
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private IRepaintingService _repaintingService;
        private IFishDataService _fishData;

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

            BindFishDataService();
            BindRepaintingService();
        }

        private void BindFishDataService()
        {
            _fishData = new FishDataService(_staticData);
            _services.RegisterSingle<IFishDataService>(_fishData);
            Container.Bind<IFishDataService>().FromInstance(_fishData).AsSingle();
        }

        #region Binding

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
            //_inputService = ChangeInputService();
            _inputService = new StandaloneInputService();
            
            Container
                .Bind<IInputService>()
                .FromInstance(_inputService)
                .AsSingle();
            _services.RegisterSingle<IInputService>(_inputService);
        }
        private void BindRepaintingService()
        {
            _repaintingService = new RepaintingService(Colorless, Colored, Container.Resolve<ScalerPaintingMask.Factory>(), _fishData);
            _services.RegisterSingle<IRepaintingService>(_repaintingService);
            Container
                .Bind<IRepaintingService>()
                .FromInstance(_repaintingService)
                .AsSingle();
        }
       
        private static IInputService ChangeInputService() =>
            Application.isEditor
                ? (IInputService) new StandaloneInputService()
                : new MobileInputService();
        #endregion

        
        public void Initialize()
        {
            CreateGame();
            
            BindGameStateMachine();
        }

        private void BindGameStateMachine()
        {
            _stateMachine = _game.StateMachine;
            
            Container
                .Bind<IGameStateMachine>()
                .FromInstance(_stateMachine)
                .AsSingle();
        }

        private void CreateGame()
        {
            _game = new Game(this
                , Instantiate(CurtainPrefab)
                , _services
                , Container);
            _game.StateMachine.Enter<BootstrapState>();

            Container.Bind<Game>().FromInstance(_game).AsSingle();
        }
    }
}