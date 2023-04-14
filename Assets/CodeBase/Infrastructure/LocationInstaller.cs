using System.Collections.Generic;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.FishCollectorService;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller, ICoroutineRunner, IInitializable
    {
        public LoadingCurtain CurtainPrefab;
        private GameBootstrapper _gameBootstrapper;
        private Game _game;
        
       public override void InstallBindings()
        {
            ResolveRepaintingService();
        }

        private void ResolveRepaintingService()
        {
            IRepaintingService repaintingService = Container.Resolve<IRepaintingService>();
            repaintingService.Init();
        }
        
        public void Initialize()
        {        
            Debug.Log("_game");

           ResolveGameBootstrapper();
        }

        private void ResolveGameBootstrapper()
        {
            _gameBootstrapper = Container.Resolve<GameBootstrapper>();
            if (_game == null)
                CreateGame();
        }

        private void CreateGame()
        {
            AllServices _services = Container.Resolve<AllServices>();
            _game = new Game(this, Instantiate(CurtainPrefab), _services);
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}