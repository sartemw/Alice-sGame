using _Project.CodeBase.Logic.Curtain;

namespace _Project.CodeBase.Infrastructure.States
{
    public class RestartLevelState : IPayloadedState<string>
    {
        private const string RestartSceneName = "RestartLevel";
        
        private readonly SceneLoader _sceneLoader;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain;

        private string _currentLevel;
        public RestartLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = curtain;
        }

        public void Enter(string sceneName)
        {
            _currentLevel = sceneName;
            _sceneLoader.Load(RestartSceneName, OnLoaded);
        }

        private void OnLoaded() => 
            _stateMachine.Enter<LoadLevelState, string>(_currentLevel);

        public void Exit() => 
            _loadingCurtain.Hide();
    }
}