using System.Threading.Tasks;
using CodeBase.Logic;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadMainMenuState : IState
    {
        private const string sceneName = "MainMenu";

        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain;
        private ISaveLoadService _saveLoadService;


        public LoadMainMenuState(GameStateMachine stateMachine ,IUIFactory uiFactory, SceneLoader sceneLoader, LoadingCurtain curtain, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _loadingCurtain = curtain;
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => 
            OnLoaded();

        public void Exit()
        {
        }

        private async void OnLoaded()
        {
            await InitUIRoot();
            InitMainMenu();
            _loadingCurtain.Hide();
        }

        private  void InitMainMenu()
        {
            _uiFactory.CreateMainMenu();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();
    }
}