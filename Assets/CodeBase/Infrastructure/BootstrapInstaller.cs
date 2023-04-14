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
    public class BootstrapInstaller : MonoInstaller
    {
        public AllServices Services;
        public IAssetProvider AssetProvider;
        public IStaticDataService StaticData;
        public IPersistentProgressService PersistentProgressService;
        public IAdsService AdsService;
        public IInputService InputService;
        public IGameFactory GameFactory;
        public IRandomService RandomService;
        public IUIFactory UiFactory;
        public ISaveLoadService SaveLoadService;
        public IWindowService WindowService;

        public GameObject FishPrefab;

        public override void InstallBindings()
        {
            BindAllServices();
            
            BindStaticDataService();
            BindAdsService();
            BindAssetProvider();
            BindInputService();
            BindRandomService();
            BindPersistentProgressService();
            
            BindUIFactory();

            BindWindowService();
      
            BindGameFactory();

            BindSaveLoadService();
            BindRepaintingService();
            
            BindFishFactory();
            BindPoolFactory();
        }

        private void BindAllServices()
        {
            Services = new AllServices(); 
            Container
                .Bind<AllServices>()
                .FromInstance(Services);
        }

        private void BindSaveLoadService()
        {
            SaveLoadService = new SaveLoadService(
                PersistentProgressService,
                GameFactory);

            Container
                .Bind<ISaveLoadService>()
                .FromInstance(SaveLoadService)
                .AsSingle();
        }

        private void BindGameFactory()
        {
            GameFactory = new GameFactory(
                InputService,
                AssetProvider,
                StaticData,
                RandomService,
                PersistentProgressService,
                WindowService,
                Services.Single<IGameStateMachine>());

            Container
                .Bind<IGameFactory>()
                .FromInstance(GameFactory)
                .AsSingle();
        }

        private void BindWindowService()
        {
            WindowService = new WindowService(UiFactory);

            Container
                .Bind<IWindowService>()
                .FromInstance(WindowService)
                .AsSingle();
        }

        private void BindUIFactory()
        {
             UiFactory = new UIFactory(
                AssetProvider,
                StaticData,
                PersistentProgressService,
                AdsService);
            
            Container
                .Bind<IUIFactory>()
                .FromInstance(UiFactory)
                .AsSingle();
        }

        private void BindPersistentProgressService()
        {
            PersistentProgressService = new PersistentProgressService();

            Container
                .Bind<IPersistentProgressService>()
                .FromInstance(PersistentProgressService)
                .AsSingle();
        }

        private void BindRandomService()
        {
            RandomService = new RandomService();
            
            Container.Bind<IRandomService>()
                .FromInstance(RandomService)
                .AsSingle();
        }
        
        private void BindStaticDataService()
        {
            StaticData = new StaticDataService();
            
            Container.Bind<IStaticDataService>()
                .FromInstance(StaticData)
                .AsSingle();

            StaticData.Load();
        }
        private void BindAdsService()
        {
            AdsService = new AdsService();
            AdsService.Initialize();
            
            Container.Bind<IAdsService>()
                .FromInstance(AdsService)
                .AsSingle();
        }
        private void BindAssetProvider()
        {
            AssetProvider = new AssetProvider();
            
            Container.Bind<IAssetProvider>()
                .FromInstance(AssetProvider)
                .AsSingle();

            AssetProvider.Initialize();
        }

        private void BindInputService()
        {
            InputService = ChangeInputService();
            
            Container
                .Bind<IInputService>()
                .FromInstance(InputService)
                .AsSingle();
        }

        private void BindRepaintingService()
        {
            Container
                .Bind<IRepaintingService>()
                .To<RepaintingService>()
                .AsSingle();
        }

        private void BindFishFactory()
        {
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
       
       
    }
}