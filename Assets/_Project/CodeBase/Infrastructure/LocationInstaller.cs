using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Repainting;
using Ami.BroAudio;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller, IInitializable
    {
        [SerializeField] private SoundID _levelMusic;
        private Game _game;

        public override void InstallBindings() => 
            BindInstaller();

        public void Initialize()
        {
            ResolvePaintingDataService();
            ResolveFishDataService();
            ResolveAudioService();
        }

        private void ResolveAudioService()
        {
            IAudioService audioService = Container.Resolve<IAudioService>();
            audioService.PlayLevelMusic(_levelMusic);
        }

        private void ResolvePaintingDataService()
        {
            IPaintingService paintingService = Container.Resolve<IPaintingService>();
            paintingService.StartLevel();
        }

        private void ResolveFishDataService()
        {
            IFishDataService fishDataService = Container.Resolve<IFishDataService>();
            fishDataService.Restart();
        }

        private void BindInstaller()
        {
            Container
                .BindInterfacesTo<LocationInstaller>()
                .FromInstance(this)
                .AsSingle();
        }
    }
}