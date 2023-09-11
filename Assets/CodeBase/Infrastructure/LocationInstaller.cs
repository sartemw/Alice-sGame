using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller, IInitializable
    {
        private Game _game;

        public override void InstallBindings()
        {
            BindInstaller();
        }


        public void Initialize()
        {
            ResolveFishDataService();
            ResolveFishRepaintableService();
        }

        private void BindInstaller()
        {
            Container
                .BindInterfacesTo<LocationInstaller>()
                .FromInstance(this)
                .AsSingle();
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