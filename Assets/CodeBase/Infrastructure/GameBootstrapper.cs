using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    public Game Game;

    public void Init(LoadingCurtain curtain, DiContainer container)
    {
      Game = new Game(this, Instantiate(curtain), container);
      Game.StateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}