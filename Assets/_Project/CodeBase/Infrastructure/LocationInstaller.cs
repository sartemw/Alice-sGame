using _Project.CodeBase.Services.Repainting;
using Zenject;

namespace _Project.CodeBase.Infrastructure
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
            ResolvePaintingDataService();
            ResolveFishDataService();
        }

        private void ResolvePaintingDataService()
        {
            IPaintingService paintingService = Container.Resolve<IPaintingService>();
            paintingService.StartLevel();
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
    }
}