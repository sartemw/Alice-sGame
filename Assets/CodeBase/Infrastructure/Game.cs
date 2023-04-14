using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public GameStateMachine StateMachine;

    
    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, AllServices services)
    {
      StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, services);
    }
  }
}