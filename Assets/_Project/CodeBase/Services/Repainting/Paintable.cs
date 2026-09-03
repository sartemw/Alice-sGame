using System;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.CodeBase.Services.Repainting
{
    public abstract class Paintable: MonoBehaviour
    {
        protected const string FadeValue = "_Fade";
        protected const string ColoredMaterial = "Colored";

        protected readonly float DeltaFadeToBright = Constants.PaintableDeltaFadeToPaint;
        protected readonly float DeltaAlphaToBright = Constants.PaintableDeltaAlphaToPaint;
        protected readonly float DeltaFadeToFade = Constants.PaintableDeltaFadeToFade;
        protected readonly float DeltaAlphaToFade = Constants.PaintableDeltaAlphaToFade;
        
        private readonly BaseOnEvent<StartPaintingSignal>  _onStartPainting  = new BaseOnEvent<StartPaintingSignal>();
        private readonly BaseOnEvent<StartPaintingInstantlySignal>  _onStartPaintingInstantlySignal  = new BaseOnEvent<StartPaintingInstantlySignal>();
        private readonly BaseOnEvent<FadeMaterialSignal>  _onFadeMaterial  = new BaseOnEvent<FadeMaterialSignal>();

        public ColorType ColorType;
        public abstract void Initialize();
        protected abstract void Brightening(StartPaintingSignal obj);
        protected abstract void BrighteningInstantly(StartPaintingInstantlySignal obj);
        protected abstract void Fade(FadeMaterialSignal obj);

        protected IPaintingService PaintingService;

        protected GameObject ColorlessObject; 
        
        public bool IsColored = true;
        
        [Inject]
        public void Construct(IPaintingService paintingService)
        {
            PaintingService = paintingService;
        }

        private void Start()
        {
            EventBus.Subscribe(_onStartPainting.SetOnInvoke(Brightening));
            EventBus.Subscribe(_onStartPaintingInstantlySignal.SetOnInvoke(BrighteningInstantly));
            EventBus.Subscribe(_onFadeMaterial.SetOnInvoke(Fade));
        }

        public void SetColorless(GameObject colorless) => 
            ColorlessObject = colorless;
        
        protected void SwitchMaterialAndColor(Renderer colorlessRenderer, GameObject colorlessObject)
        {
            colorlessRenderer.sortingOrder += 1;
            Paintable colorlessPaintable = colorlessObject.GetComponent<Paintable>();

            colorlessPaintable.IsColored = false;
            colorlessPaintable.SetColorless(colorlessObject);
            PaintingService.SetColorless(colorlessPaintable);

            colorlessRenderer.material = PaintingService.ColorlessMaterial;

            if (colorlessRenderer is SpriteRenderer)
                colorlessRenderer.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}