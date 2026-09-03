using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Localization;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.SaveLoad;
using _Project.CodeBase.StaticData;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Windows
{
    public class GameMenuWindow : WindowBase
    {
        private const string InitialLevel = "MainMenu";
        
        public Button ShowAdButton;
        public Button RestartLevelButton;
        public Button MainMenuButton;
        public Button CheatsButton;
        public Button SoundButton;
        public Button LanguageButton;

        public Sprite SoundImageOn;
        public Sprite SoundImageOff;
        public Image SoundIconImage;
        public bool IsSoundOn;

        private ConfigStaticData _config;
        private IGameStateMachine _stateMachine;
        private ILocalizationService _localizationService;
        private ISaveLoadService _saveLoadService;
        private IAudioService _audioService;

        public void Construct(
            IPersistentProgressService progressService,
            IGameStateMachine stateMachine,
            ConfigStaticData config,
            ILocalizationService localizationService,
            ISaveLoadService saveLoadService,
            IAudioService audioService)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
            _config = config;
            _localizationService = localizationService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
        }

        protected override void Initialize()
        {
            IsSoundOn = _config.Sound;
            ChangeSoundIcon();
            _localizationService.SetLanguage(LocalizationSettings.SelectedLocale.Identifier.Code);
            
            ShowAdButton.onClick.AddListener(OnShowAdClicked);
            RestartLevelButton.onClick.AddListener(OnRestartLevelClicked);
            MainMenuButton.onClick.AddListener(OnLoadMainMenuClicked);
            LanguageButton.onClick.AddListener(OnLanguageClicked);
            SoundButton.onClick.AddListener(OnSoundClicked);
            
            CheatsButton.gameObject.SetActive(_config.IsDebug);
        }

        private void OnSoundClicked()
        {
            IsSoundOn = !IsSoundOn;

            ChangeSoundIcon();
            
            EventBus.Invoke(new SoundButtonClickSignal());
            
            _saveLoadService.SaveConfig();
        }

        private void ChangeSoundIcon()
        {
            if (IsSoundOn)
                SoundIconImage.sprite = SoundImageOn;
            else
                SoundIconImage.sprite = SoundImageOff;
        }

        private void OnLanguageClicked()
        {
            string localeCode;
            
            if (LocalizationSettings.SelectedLocale.Identifier == "en")
                localeCode = "ru";
            else
                localeCode = "en";

            _localizationService.SetLanguage(localeCode);
            
            _saveLoadService.SaveConfig();
        }
        
        

        private void OnLoadMainMenuClicked() => 
            _stateMachine.Enter<LoadMainMenuState, string>(InitialLevel);

        private void OnRestartLevelClicked()
        {
            _stateMachine.Enter<RestartLevelState, string>(SceneManager.GetActiveScene().name);
            Close();
        }

        private void OnShowAdClicked() => 
            Debug.Log("<color=cyan>Show AD</color>");

        private void Close()
        {
            _audioService.PlayOpenWindow();
            Destroy(gameObject);
        }
    }
}