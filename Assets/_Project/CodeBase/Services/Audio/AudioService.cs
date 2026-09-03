using _Project.CodeBase.Events;
using _Project.CodeBase.Services.PersistentConfig;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using Ami.BroAudio;
using UnityEngine;

namespace _Project.CodeBase.Services.Audio
{
    public class AudioService: IAudioService
    {
        private BaseOnEvent<SoundButtonClickSignal>  _onSoundButtonClick  = new BaseOnEvent<SoundButtonClickSignal>();
        private BaseOnEvent<PlaySoundSignal>  _onPlaySound  = new BaseOnEvent<PlaySoundSignal>();

        private readonly IStaticDataService _staticData;
        private readonly ConfigStaticData _config;

        private SoundID _currentSoundId;

        private bool CanPlay
        {
            get
            {
                if (_config.Sound)
                    BroAudio.SetVolume(0.25f);
                else
                    BroAudio.SetVolume(0);
                
                return _config.Sound;
            }
            set { value = _config.Sound; }
        }

        public AudioService(ConfigStaticData config, IStaticDataService staticData)
        {
            _config = config;
            _staticData = staticData;
        }

        public void Init()
        {
            EventBus.Subscribe(_onSoundButtonClick.SetOnInvoke(OnSoundButtonClick));
            EventBus.Subscribe(_onPlaySound.SetOnInvoke(OnPlaySound));
        }

        public void PlayLevelMusic(SoundID music)
        {
            if (music == _currentSoundId)
                return;
            
            PlayMusic(music);
        }

        public void PlayBlobs() => 
            PlaySound(_staticData.ForAudio().Blobs);
        public void PlayOpenWindow() => 
            PlaySound(_staticData.ForAudio().OpenWindow);
        public void PlayGameMenuOpen() => 
            PlaySound(_staticData.ForAudio().GameMenuOpen);
        public void PlayCatJump() => 
            PlaySound(_staticData.ForAudio().CatJump);
        public void PlayPickupFish() => 
            PlaySound(_staticData.ForAudio().PickupFish);
        public void PlayLoseLevel() => 
            PlaySound(_staticData.ForAudio().LoseLevel);
        public void PlayOpenDoor() => 
            PlaySound(_staticData.ForAudio().OpenDoor);
        
        private void OnPlaySound(PlaySoundSignal obj) =>
            PlaySound(obj.Sound);

        private void OnSoundButtonClick(SoundButtonClickSignal obj)
        {
            _config.Sound = !_config.Sound;
            PlayMusic(_currentSoundId);
        }

        private void PlayMusic(SoundID music)
        {
            _currentSoundId = music;
            BroAudio.Stop(_currentSoundId);
            
            if (!CanPlay)
                return;
            Debug.Log($"Playing music {_currentSoundId.ToName()}");
            BroAudio.Play(_currentSoundId);
        }
        
        private void PlaySound(SoundID music)
        {
            if (!CanPlay)
                return;
            
            BroAudio.Play(music);
        }

        public void LoadConfig(ConfigData config) => 
            CanPlay = config.Sound;

        public void UpdateConfig(ConfigData config) => 
            config.Sound = CanPlay;
    }
}