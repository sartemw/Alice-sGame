using CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class FactoryInstaller : MonoInstaller
    {
        public GameObject FishPrefab;
        public GameObject PaintingMask;
        
        public override void InstallBindings()
        {
            BindFishFactory();
            BindPoolFactory();
            BindPaintingMaskFactory();
        }

        private void BindPaintingMaskFactory()
        {
            Container
                .BindFactory<ScalerPaintingMask, ScalerPaintingMask.Factory>()
                .FromComponentInNewPrefab(PaintingMask);
        }

        private void BindFishFactory()
        {
            Container
                .BindFactory<ColoredFish, ColoredFish.Factory>()
                .FromComponentInNewPrefab(FishPrefab);
        }
        private void BindPoolFactory()
        {
            Container
                .Bind<IPoolFactory>()
                .To<PoolFactory>()
                .AsSingle();
        }
    }
}