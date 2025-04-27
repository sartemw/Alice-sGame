using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Services.Repainting
{
    public abstract class Paintable: MonoBehaviour
    {
        private BaseOnEvent<StartPaintingSignal>  _onStartPainting  = new BaseOnEvent<StartPaintingSignal>();
        
        public ColorType ColorType;
        
        public abstract void Initialize();
        protected abstract void Brightening(StartPaintingSignal obj);

        protected IPaintingService PaintingService;
        protected GameObject Colorless;

        protected float DeltaFade = Constants.PaintableDeltaFade;
        protected float DeltaAlpha = Constants.PaintableDeltaAlpha;
        
        [Inject]
        public void Construct(IPaintingService paintingService)
        {
            PaintingService = paintingService;
            EventBus.Subscribe(_onStartPainting.SetOnInvoke(Brightening));
        }


        public void SetColorless(GameObject colorless) => 
            Colorless = colorless;

    }
}