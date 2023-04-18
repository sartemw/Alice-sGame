using CodeBase.Fish;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Ads;
using CodeBase.Services.FishCollectorService;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ICoroutineRunner
    {
        private Game _game;
        private AllServices _services;
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticData;
        private IInputService _inputService;
        private IAdsService _adsService;
        public GameObject FishPrefab;
        public LoadingCurtain CurtainPrefab;

        public override void InstallBindings()
        {
            BindBootstrapInstaller();
            BindAllServices();

            BindStaticDataService();
            BindAdsService();
            BindAssetProvider();
            BindInputService();
            BindFishCollectorService();
            
            BindFishFactory();
            BindPoolFactory();
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
        }

        private void CreateGame()
        {
            _game = new Game(this, Instantiate(CurtainPrefab), _services);
            _game.StateMachine.Enter<BootstrapState>();
            
            Container.Bind<Game>().FromInstance(_game).AsSingle();
        }
    }
}