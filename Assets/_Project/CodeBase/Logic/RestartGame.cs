using _Project.CodeBase.Infrastructure;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.Logic
{
    public class RestartGame : MonoBehaviour
    {
        public Button RestartGameButton;
        private IPersistentProgressService _persistentProgressService;
        private GameStateMachine _stateMachine;

        private void Awake() => 
            RestartGameButton.onClick.AddListener(RestartGameClecked);

        private void RestartGameClecked()
        {
            _stateMachine = FindFirstObjectByType<BootstrapInstaller>()._game.StateMachine;
            _persistentProgressService = FindFirstObjectByType<BootstrapInstaller>()._persistentProgress;
            _persistentProgressService.PlayerProgress.GameProgressData.RestartGame();
            
            _stateMachine.Enter<LoadLevelState, string>("0-1");
        }

    }
}