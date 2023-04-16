using CodeBase.Fish;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.FishCollectorService;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable, ICoroutineRunner
    {
        public Game Game;
        public AllServices Services;
        public IAssetProvider AssetProvider;
        public IStaticDataService StaticData;
        public IInputService InputService;

        public GameObject FishPrefab;
        public LoadingCurtain CurtainPrefab;

        public override void InstallBindings()
        {
            BindBootstrapInstaller();
            BindAllServices();

            BindStaticDataService();
            BindAssetProvider();
            BindInputService();
            BindFishCollectorService();
            
            BindFishFactory();
            BindPoolFactory();
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
            Services = new AllServices();

            Container
                .Bind<AllServices>()
                .FromInstance(Services)
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            StaticData = new StaticDataService();
            
            Container.Bind<IStaticDataService>()
                .FromInstance(StaticData)
                .AsSingle();
            
            Services.RegisterSingle<IStaticDataService>(StaticData);

            StaticData.Load();
        }

        private void BindAssetProvider()
        {
            AssetProvider = new AssetProvider();
            
            Container.Bind<IAssetProvider>()
                .FromInstance(AssetProvider)
                .AsSingle();
            
            Services.RegisterSingle<IAssetProvider>(AssetProvider);

            AssetProvider.Initialize();
        }

        private void BindInputService()
        {
            InputService = ChangeInputService();
            
            Container
                .Bind<IInputService>()
                .FromInstance(InputService)
                .AsSingle();
            Services.RegisterSingle<IInputService>(InputService);
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
        
        public void CreateGame()
        {
            Game = new Game(this, Instantiate(CurtainPrefab), Services);
            Game.StateMachine.Enter<BootstrapState>();
            
            Container.Bind<Game>().FromInstance(Game).AsSingle();
        }
    }
}