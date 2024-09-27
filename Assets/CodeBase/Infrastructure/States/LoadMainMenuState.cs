using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadMainMenuState : IPayloadedState<string>
    {
        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain;
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;


        public LoadMainMenuState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            LoadingCurtain curtain, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = curtain;
            _progressService = diContainer.Resolve<IPersistentProgressService>();
            _uiFactory = diContainer.Resolve<IUIFactory>();
            _gameFactory = diContainer.Resolve<IGameFactory>();
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitMainMenu();
            
            InformProgressReaders();
            
            _stateMachine.Enter<GameLoopState>();
            //_stateMachine.Enter<LoadProgressState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
        
        private async Task InitMainMenu() => 
            await _uiFactory.CreateMainMenu();

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();
    }
}