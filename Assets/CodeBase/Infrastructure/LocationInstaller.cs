using CodeBase.Services.Repainting;
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
            ResolveRepaintingService();
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

        private void ResolveRepaintingService()
        {
            IRepaintingService repaintingService = Container.Resolve<IRepaintingService>();
            repaintingService.Restart();
        }
    }
}