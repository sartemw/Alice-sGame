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

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, AllServices services, DiContainer diContainer)
    {
      StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, services, diContainer);
    }
  }
}