using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.Curtain;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class GameStateMachine : IGameStateMachine
  {
    private Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
      DiContainer diContainer)
    {
      _states = new Dictionary<Type, IExitableState>
      {
        [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, diContainer),
        
        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, diContainer),
        
        [typeof(LoadProgressState)] = new LoadProgressState(this, diContainer),
        
        [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, sceneLoader, loadingCurtain, diContainer),
        
        [typeof(GameLoopState)] = new GameLoopState(this),
      };
    }
    
    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
      TState state = ChangeState<TState>();

      state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
      _activeState?.Exit();
      
      TState state = GetState<TState>();

      _activeState = state;
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState => 
      _states[typeof(TState)] as TState;
  }
}