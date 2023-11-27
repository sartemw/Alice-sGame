using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
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
        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, DiContainer container)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, container.Resolve<IGameFactory>(),
                    container.Resolve<IPersistentProgressService>(), container.Resolve<IStaticDataService>(), container.Resolve<IUIFactory>()),
        
<<<<<<< HEAD
        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain
          ,services.Single<IGameFactory>()
          ,services.Single<IPersistentProgressService>()
          , services.Single<IStaticDataService>()
          , services.Single<IUIFactory>()),
        
        [typeof(LoadProgressState)] = new LoadProgressState(this
          ,services.Single<IPersistentProgressService>()
          ,services.Single<ISaveLoadService>()),
        
        [typeof(LoadMainMenuState)] = new LoadMainMenuState(this
          ,services.Single<IUIFactory>()
          , sceneLoader
          , loadingCurtain
          ,services.Single<ISaveLoadService>()),
        
        [typeof(GameLoopState)] = new GameLoopState(this),
      };
    }
=======
                [typeof(LoadProgressState)] = new LoadProgressState(this, container.Resolve<IPersistentProgressService>(), container.Resolve<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
                [typeof(LoadMainMenuState)] = new LoadMainMenuState(this
                    ,container.Resolve<IUIFactory>()
                    , sceneLoader
                    , loadingCurtain
                    ,container.Resolve<ISaveLoadService>()),
            };
        }
>>>>>>> 884faa757ea49c0624f6142f92af3e27e5492eb9
    
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