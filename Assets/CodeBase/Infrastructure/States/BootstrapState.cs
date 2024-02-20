using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private const string Initial = "Initial";
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
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

    public void Exit()
    {
    }

    private void RegisterServices()
    {
      _diContainer
        .Bind<IGameStateMachine>()
        .FromInstance(_stateMachine)
        .AsSingle();

      BindGameFactory();
      BindSaveLoadService();
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

    private void BindGameFactory()
    {
      IGameFactory gameFactory = new GameFactory
      (_diContainer.Resolve<IInputService>(),
        _diContainer.Resolve<IAssetProvider>(),
        _diContainer.Resolve<IStaticDataService>(),
        _diContainer.Resolve<IRandomService>(),
        _diContainer.Resolve<IPersistentProgressService>(),
        _diContainer.Resolve<IWindowService>(),
        _stateMachine,
        _diContainer);

      _diContainer
        .Bind<IGameFactory>()
        .FromInstance(gameFactory)
        .AsSingle();
    }

    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadProgressState>();
  }
}