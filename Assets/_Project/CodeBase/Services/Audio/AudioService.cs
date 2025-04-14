using _Project.CodeBase.Events;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using Ami.BroAudio;
using UnityEngine;

namespace _Project.CodeBase.Services.Audio
{
    public class AudioService: IAudioService
    {
        private BaseOnEvent<EnterMainMenuSignal>  _onEnterMainMenu  = new BaseOnEvent<EnterMainMenuSignal>();
        private BaseOnEvent<SoundButtonClickSignal>  _onSoundButtonClick  = new BaseOnEvent<SoundButtonClickSignal>();
        private BaseOnEvent<LoadLevelSignals>  _onLevelLoad  = new BaseOnEvent<LoadLevelSignals>();

        private readonly IStaticDataService _staticData;
        private readonly ConfigStaticData _config;

        private SoundID _currentSoundId;

        private bool CanPlay
        {
            get
            {
                if (_config.Sound)
                    BroAudio.SetVolume(1);
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
            EventBus.Subscribe(_onEnterMainMenu.SetOnInvoke(OnEnterMainMenu));
            EventBus.Subscribe(_onSoundButtonClick.SetOnInvoke(OnSoundButtonClick));
            EventBus.Subscribe(_onLevelLoad.SetOnInvoke(OnLevelLoad));
        }

        private void OnSoundButtonClick(SoundButtonClickSignal obj)
        {
            CanPlay = !CanPlay;
            
            PlayMusic(_currentSoundId);
        }

        private void OnLevelLoad(LoadLevelSignals obj) =>
            PlayMusic(_staticData.ForAudio().Game1);

        private void OnEnterMainMenu(EnterMainMenuSignal obj) => 
            PlayMusic(_staticData.ForAudio().MainMenu);

        private void PlayMusic(SoundID music)
        {
            _currentSoundId = music;
            BroAudio.Stop(_currentSoundId);
            
            if (!CanPlay)
                return;
            
            BroAudio.Play(_currentSoundId);
        }
    }
}