using _Project.CodeBase.Services.Audio;
using Ami.BroAudio;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
{
    public class MenuInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private SoundID _levelMusic;
        
        public override void InstallBindings() => 
            BindInstaller();

        public void Initialize()
        {
            ResolveAudioService();
        }
        
        private void ResolveAudioService()
        {
            IAudioService audioService = Container.Resolve<IAudioService>();
            audioService.PlayLevelMusic(_levelMusic);
        }
        
        private void BindInstaller()
        {
            Container
                .BindInterfacesTo<MenuInstaller>()
                .FromInstance(this)
                .AsSingle();
        }
    }
}