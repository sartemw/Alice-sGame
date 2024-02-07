using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadMainMenuState : IPayloadedState<string>
    {
        private const string sceneName = "MainMenu";

        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;


        public LoadMainMenuState(GameStateMachine stateMachine, IUIFactory uiFactory, SceneLoader sceneLoader,
            LoadingCurtain curtain, ISaveLoadService saveLoadService, IGameFactory gameFactory)
        {
            _saveLoadService = saveLoadService;
            _loadingCurtain = curtain;
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            InitMainMenu();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitMainMenu() => 
             _uiFactory.CreateMainMenu();

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();
    }
}