using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, DiContainer diContainer) => 
      StateMachine = new GameStateMachine(InitSceneLoader(coroutineRunner, diContainer), curtain, diContainer);

    private static SceneLoader InitSceneLoader(ICoroutineRunner coroutineRunner, DiContainer diContainer)
    {
      SceneLoader sceneLoader = new SceneLoader(coroutineRunner);
      diContainer.Bind<SceneLoader>().FromInstance(sceneLoader).AsSingle();
      return sceneLoader;
    }
  }
}