using System;
using System.Collections.Generic;
using _Project.CodeBase.Events;
using _Project.CodeBase.Logic.Curtain;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
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
        
        [typeof(LoadCutsceneState)] = new LoadCutsceneState(this, sceneLoader, loadingCurtain, diContainer),
        
        [typeof(LoadProgressState)] = new LoadProgressState(this, diContainer),
        
        [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, sceneLoader, loadingCurtain, diContainer),
        
        [typeof(RestartLevelState)] = new RestartLevelState(this, sceneLoader, loadingCurtain),

        [typeof(GameLoopState)] = new GameLoopState(this),
      };
      
      EventBus.Subscribe(_onNextSceneSignal.SetOnInvoke(NextSceneLoad));
    }

    private void NextSceneLoad(NextSceneSignal obj)
    {
      Enter<LoadLevelState, string>(obj.Scene);
    }

    private BaseOnEvent<NextSceneSignal>  _onNextSceneSignal  = new BaseOnEvent<NextSceneSignal>();
    
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