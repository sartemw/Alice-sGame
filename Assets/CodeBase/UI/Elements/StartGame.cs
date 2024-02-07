using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class StartGame : MonoBehaviour
    {
        public Button Button;
        public string LoadLevel = "0-1";
        
        private IGameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        
        public void Construct (IGameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }

        private void Awake() => 
            Button.onClick.AddListener(StartLevel);

        private void StartLevel()
        {
            _sceneLoader.Load(LoadLevel, onLoaded: EnterLoadLevel);
        }
        
        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadLevelState, string>(LoadLevel);
    }
}