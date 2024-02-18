using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class LoadLevelButton : MonoBehaviour
    {
        public Button Button;
        public string LoadLevel;
        
        private GameStateMachine _stateMachine;

        public void Init(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Awake() => 
            Button.onClick.AddListener(StartLevel);

        private void StartLevel() => 
            _stateMachine.Enter<LoadLevelState, string>(LoadLevel);
    }
}