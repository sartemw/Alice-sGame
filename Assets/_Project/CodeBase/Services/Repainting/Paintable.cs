using System;
using System.Collections;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Services.Repainting
{
    public abstract class Paintable: MonoBehaviour
    {
        protected const string FadeValue = "_Fade";
        protected const string ColoredMaterial = "Colored";

        private BaseOnEvent<StartPaintingSignal>  _onStartPainting  = new BaseOnEvent<StartPaintingSignal>();
        private BaseOnEvent<StartPaintingInstantlySignal>  _onStartPaintingInstantlySignal  = new BaseOnEvent<StartPaintingInstantlySignal>();
        private BaseOnEvent<FadeMaterialSignal>  _onFadeMaterial  = new BaseOnEvent<FadeMaterialSignal>();

        public ColorType ColorType;
        public abstract void Initialize();
        protected abstract void Brightening(StartPaintingSignal obj);
        protected abstract void BrighteningInstantly(StartPaintingInstantlySignal obj);
        protected abstract void Fade(FadeMaterialSignal obj);

        protected IPaintingService PaintingService;
        protected GameObject ColorlessObject;

        protected float DeltaFadeToBright = Constants.PaintableDeltaFadeToPaint;
        protected float DeltaAlphaToBright = Constants.PaintableDeltaAlphaToPaint;
        protected float DeltaFadeToFade = Constants.PaintableDeltaFadeToFade;
        protected float DeltaAlphaToFade = Constants.PaintableDeltaAlphaToFade;
        
        [Inject]
        public void Construct(IPaintingService paintingService)
        {
            PaintingService = paintingService;
            // EventBus.Subscribe(_onStartPainting.SetOnInvoke(Brightening));
            // EventBus.Subscribe(_onFadeMaterial.SetOnInvoke(Fade));
        }

        private void Start()
        {
            EventBus.Subscribe(_onStartPainting.SetOnInvoke(Brightening));
            EventBus.Subscribe(_onFadeMaterial.SetOnInvoke(Fade));
            EventBus.Subscribe(_onStartPaintingInstantlySignal.SetOnInvoke(BrighteningInstantly));
        }

        public void SetColorless(GameObject colorless) => 
            ColorlessObject = colorless;
        
        protected void SwitchMaterialAndColor(Renderer colorlessRenderer, GameObject colorlessObject)
        {
            Paintable colorlessPaintable = colorlessObject.GetComponent<Paintable>();

            colorlessPaintable.SetColorless(colorlessObject);
            PaintingService.SetColorless(colorlessPaintable);

            colorlessRenderer.material = PaintingService.ColorlessMaterial;

            if (colorlessRenderer is SpriteRenderer)
                colorlessRenderer.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}