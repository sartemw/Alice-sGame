﻿using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.FishCollectorService;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class LocationInstaller: MonoInstaller, IInitializable
    {
        private Game _game;

        public override void InstallBindings()
        {
            ResolveFishCollectorService();

            Container.Bind<LocationInstaller>().FromInstance(this).AsSingle();
        }
        
        private void ResolveFishCollectorService()
        {
            IRepaintingService repaintingService = Container.Resolve<IRepaintingService>();
            repaintingService.Init();
        }

        public void Initialize()
        {
        }
    }
}