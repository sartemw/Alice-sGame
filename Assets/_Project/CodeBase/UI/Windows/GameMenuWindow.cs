using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;
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

        private IGameStateMachine _stateMachine;

        public void Construct(IPersistentProgressService progressService, IGameStateMachine stateMachine)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
        }

        protected override void Initialize()
        {
            ShowAdButton.onClick.AddListener(OnShowAdClicked);
            RestartLevelButton.onClick.AddListener(OnRestartLevelClicked);
            MainMenuButton.onClick.AddListener(OnLoadMainMenuClicked);
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

        private void Close() => 
            Destroy(gameObject);
    }
}