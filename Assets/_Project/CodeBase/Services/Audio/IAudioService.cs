using _Project.CodeBase.Services.PersistentConfig;
using Ami.BroAudio;

namespace _Project.CodeBase.Services.Audio
{
    public interface IAudioService: IService, ISavedConfig
    {
        public void Init();
        public void PlayLevelMusic(SoundID music);
        public void PlayBlobs();
        public void PlayOpenWindow();
        public void PlayGameMenuOpen();
        public void PlayCatJump();
        public void PlayPickupFish();
        public void PlayLoseLevel();
        public void PlayOpenDoor();
    }
}