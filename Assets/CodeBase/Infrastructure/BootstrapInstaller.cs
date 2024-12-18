﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.Curtain;
using CodeBase.Mask;
using CodeBase.Services.Ads;
using CodeBase.Services.Audio;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Repainting;
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
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticData;
        private IInputService _inputService;
        private IAdsService _adsService;
        private IRandomService _randomService;
        private IPersistentProgressService _persistentProgress;
        private IUIFactory _uiFactory;
        private IWindowService _windowService;
        private IRepaintingService _repaintingService;
        private IFishDataService _fishData;
        private IAudioService _audioService;
        private IAudioAssetsService _audioAssetsService;

        public override void InstallBindings()
        {
            BindBootstrapInstaller();
            BindStaticDataService();
            //BindAdsService();
            BindAssetProvider();
            BindInputService();
            BindRandomService();
            BindPersistentProgressService();
            BindUIFactory();
            BindWindowService();

            BindFishDataService();
            BindRepaintingService(); 
            
            BindAudioAssetService();
            BindAudioService();
        }

        #region Binding

        private void BindFishDataService()
        {
            _fishData = new FishDataService(_staticData);
            Container.Bind<IFishDataService>().FromInstance(_fishData).AsSingle();
        }

        private void BindWindowService()
        {
            _windowService = new WindowService(_uiFactory);
            Container
                .Bind<IWindowService>()
                .FromInstance(_windowService)
                .AsSingle();
        }

        private void BindUIFactory()
        {
            _uiFactory = new UIFactory(_assetProvider, _staticData, _persistentProgress, _adsService, Container);
            Container
                .Bind<IUIFactory>()
                .FromInstance(_uiFactory)
                .AsSingle();
        }

        private void BindPersistentProgressService()
        {
            _persistentProgress = new PersistentProgressService();
            Container
                .Bind<IPersistentProgressService>()
                .FromInstance(_persistentProgress)
                .AsSingle();
        }

        private void BindRandomService()
        {
            _randomService = new RandomService();
            Container
                .Bind<IRandomService>()
                .FromInstance(_randomService)
                .AsSingle();
        }

        // private void BindAdsService()

        // {

        //     _adsService = new AdsService();

        //     _adsService.Initialize();

        //     Container

        //         .Bind<IAdsService>()

        //         .FromInstance(_adsService)

        //         .AsSingle();

        // }

        private void BindBootstrapInstaller()
        {
            Container
                .BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            _staticData = new StaticDataService();

            Container.Bind<IStaticDataService>()
                .FromInstance(_staticData)
                .AsSingle();

            _staticData.Load();
        }

        private void BindAssetProvider()
        {
            _assetProvider = new AssetProvider();

            Container.Bind<IAssetProvider>()
                .FromInstance(_assetProvider)
                .AsSingle();

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
        }

        private void BindRepaintingService()
        {
            _repaintingService = new RepaintingService(Colorless, Colored,
                Container.Resolve<ScalerPaintingMask.Factory>(), _fishData);
            Container
                .Bind<IRepaintingService>()
                .FromInstance(_repaintingService)
                .AsSingle();
        }

        private void BindAudioAssetService()
        {
            _audioAssetsService = new AudioAssetsService();
            Container
                .Bind<IAudioAssetsService>()
                .FromInstance(_audioAssetsService)
                .AsSingle();
            
            _audioAssetsService.Load();
        }

        private void BindAudioService()
        {
            _audioService = new AudioService(GetComponent<AudioSource>(), _audioAssetsService);
            Container
                .Bind<IAudioService>()
                .FromInstance(_audioService)
                .AsSingle();
        }

        private static IInputService ChangeInputService() =>
            Application.isEditor
                ? (IInputService) new StandaloneInputService()
                : new MobileInputService();

        #endregion


        public void Initialize() =>
            CreateGame();

        private void CreateGame()
        {
            _game = new Game(this
                , Instantiate(CurtainPrefab)
                , Container);
            Container.Bind<Game>().FromInstance(_game).AsSingle();

            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}