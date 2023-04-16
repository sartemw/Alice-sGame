using System.Collections.Generic;
using CodeBase.Services.FishCollectorService;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller
    { 
        public List<GameObject> ColorlessObjs;
        public List<GameObject> ColorledObjs;

        public override void InstallBindings()
        {
            ResolveFishCollectorService();
        }


        private void ResolveFishCollectorService()
        {
            IRepaintingService repaintingService = Container.Resolve<IRepaintingService>();
            repaintingService.Init();
            ColorlessObjs = repaintingService.ColorlessObjs;
            ColorledObjs = repaintingService.ColorledObjs;
        }

    }
}