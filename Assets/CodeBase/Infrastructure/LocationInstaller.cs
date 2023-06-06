using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller
    {
        private Game _game;

        public override void InstallBindings()
        {
            ResolveFishDataService();
            ResolveFishRepaintableService();
        }

        private void ResolveFishDataService()
        {
            IFishDataService fishDataService = Container.Resolve<IFishDataService>();
            fishDataService.Restart();
        }

        private void ResolveFishRepaintableService()
        {
            IRepaintingService repaintingService = Container.Resolve<IRepaintingService>();
            repaintingService.Restart();
        }

    }
}