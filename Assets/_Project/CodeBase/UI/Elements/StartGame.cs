using _Project.CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Elements
{
    public class StartGame : MonoBehaviour
    {
        public Button Button;
        public string LoadLevel = "0-1";
        
        private IGameStateMachine _stateMachine;
        
        public void Construct (IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        private void Awake() => 
            Button.onClick.AddListener(StartLevel);

        private void StartLevel() => 
            _stateMachine.Enter<LoadLevelState, string>(LoadLevel);
    }
}