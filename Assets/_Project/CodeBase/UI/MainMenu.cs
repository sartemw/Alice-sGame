using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.UI.Elements;
using _Project.CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI
{
    public class MainMenu : WindowBase
    {
        private const string MusicTrack = "MainMenu";
        public Button SoundButton;
        public Button StartButton;
        private GameStateMachine _stateMachine;
        private ConfigStaticData _config;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine, ConfigStaticData config)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
            _config = config;
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
            _config.Sound = !_config.Sound;
            EventBus.Invoke(new SoundButtonClickSignal());
        }

        void StartClick()
        {
            EventBus.Invoke(new StartGameButtonClickSignal());

            _stateMachine.Enter<LoadLevelState, string>(CurrentLevel());
        }


        private void Exit()
        {
            Cleanup();
            Application.Quit();
        }
        
        private string CurrentLevel()
        {
            int levelsCompleted = Progress.GameProgressData.CurrentLevel;
            
            return $"0-{levelsCompleted}";
        }
    }
}