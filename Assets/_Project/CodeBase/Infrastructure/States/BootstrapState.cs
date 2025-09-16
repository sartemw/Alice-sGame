using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.AssetManagement;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Services.Analytics;
using _Project.CodeBase.Services.Input;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.Randomizer;
using _Project.CodeBase.Services.SaveLoad;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.UI.Services.Windows;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly DiContainer _diContainer;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader,
      DiContainer diContainer)
    {
      _diContainer = diContainer;
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      
      RegisterServices();
    }

    public void Enter() => 
      EnterLoadLevel();

    public void Exit() {}

      private void RegisterServices()
    {
      BindStateMachine();
      BindGameFactory();
      BindSaveLoadService();
      
      EventBus.Invoke(new BootstrapFinishedSignal());
    }

      private void BindStateMachine()
      {
        _diContainer
          .Bind<IGameStateMachine>()
          .FromInstance(_stateMachine)
          .AsSingle();
      }
      private void BindGameFactory()
    {
      IGameFactory gameFactory = new GameFactory
      (_diContainer.Resolve<IInputService>(),
        _diContainer.Resolve<IAssetProvider>(),
        _diContainer.Resolve<IStaticDataService>(),
        _diContainer.Resolve<IRandomService>(),
        _diContainer.Resolve<IPersistentProgressService>(),
        _diContainer.Resolve<IWindowService>(),
        _diContainer.Resolve<IAnalyticsService>(),
        _stateMachine,
        _diContainer);

      _diContainer
        .Bind<IGameFactory>()
        .FromInstance(gameFactory)
        .AsSingle();
    }
      private void BindSaveLoadService()
    {
      ISaveLoadService saveLoadService = new SaveLoadService(
        _diContainer.Resolve<IPersistentProgressService>(),
        _diContainer.Resolve<IGameFactory>());

      _diContainer
        .Bind<ISaveLoadService>()
        .FromInstance(saveLoadService)
        .AsSingle();
    }

    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadProgressState>();
  }
}