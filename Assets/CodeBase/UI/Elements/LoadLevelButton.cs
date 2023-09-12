using CodeBase.Infrastructure.States;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class LoadLevelButton : MonoBehaviour
    {
        public Button Button;
        public string LoadLevel;
        
        private GameStateMachine _stateMachine;

        public void Init(GameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        private void Awake() => 
            Button.onClick.AddListener(Open);

        private void Open() => 
            _stateMachine.Enter<LoadLevelState, string>(LoadLevel);
    }
}