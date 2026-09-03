using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.SaveLoad;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;

namespace _Project.CodeBase.UI
{
    public class MainMenu : WindowBase
    {
        private const string MusicTrack = "MainMenu";
        public Button SoundButton;
        public Button StartButton;
        public Button CheatsButton;
        private GameStateMachine _stateMachine;
        private ConfigStaticData _config;
        private ISaveLoadService _saveLoadService;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine, ConfigStaticData config, ISaveLoadService saveLoadService)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
            _config = config;
            _saveLoadService = saveLoadService;

            CheatsButton.gameObject.SetActive(_config.IsDebug);
        }

        private void Start()
        {
            EventBus.Invoke(new EnterMainMenuSignal());
            
            SoundButton.onClick.AddListener(SoundClick);
            StartButton.onClick.AddListener(StartClick);
            CloseButton.onClick.AddListener(CloseClick);
        }

        private void CloseClick()
        {
            EventBus.Invoke(new CloseButtonClickSignal());
            
            Debug.Log("<color=red> Application quit</color>");
            //Application.Quit();
        }

        private void SoundClick()
        {
            EventBus.Invoke(new SoundButtonClickSignal());
            _saveLoadService.SaveConfig();
        }

        void StartClick()
        {
            EventBus.Invoke(new StartGameButtonClickSignal());

            _stateMachine.Enter<LoadLevelState, string>(CurrentLevel());
        }


        private void Exit()
        {
            //Cleanup();
            Application.Quit();
        }
        
        private string CurrentLevel()
        {
            int levelsCompleted = Progress.GameProgressData.CurrentLevel;
            
            return $"0-{levelsCompleted}";
        }
    }
}