using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.UI.Elements;
using _Project.CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI
{
    public class MainMenu : WindowBase
    {
        public Button SoundButton;
        public Button StartButton;
        private GameStateMachine _stateMachine;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
        }

        private void Start()
        {
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
            Debug.Log("Выключить в конфиге звук");
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
            int levelsCompleted = Progress.GameProgressData.LevelsCompleted;
            
            return $"0-{levelsCompleted}";
        }
    }
}