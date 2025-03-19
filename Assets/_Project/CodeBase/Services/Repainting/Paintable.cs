using _Project.CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Services.Repainting
{
    public abstract class Paintable: MonoBehaviour
    {
        public ColorType ColorType;

        public abstract  void Initialize();
        public abstract  void Fade();
        public abstract void Brightening();
        
        protected IPaintingService PaintingService;
        protected GameObject Colorless;

        protected float DeltaFade = Constants.PaintableDeltaFade;
        protected float DeltaAlpha = Constants.PaintableDeltaAlpha;
        
        [Inject]
        public void Construct(IPaintingService paintingService) => 
            PaintingService = paintingService;
        
        public void SetColorless(GameObject colorless) => 
            Colorless = colorless;

    }
}