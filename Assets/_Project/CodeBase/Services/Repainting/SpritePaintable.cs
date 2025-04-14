using System.Collections;
using _Project.CodeBase.Events;
using UnityEngine;

namespace _Project.CodeBase.Services.Repainting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePaintable: Paintable
    {
        private const string FadeValue = "_Fade";
        private SpriteRenderer _renderer;
        
        private SpriteRenderer _colorlessRenderer;
        
        public override void Initialize()
        {
            ColoredSetup();
            ColorlessSetup();
        }

        public override void Fade() => 
            StartCoroutine(FadeSprite());

        protected override void Brightening(StartPaintingSignal obj)
        {
            if (obj.Target == this)
                StartCoroutine(BrighteningSprite());
        }

        private IEnumerator FadeSprite()
        {
            SpriteRenderer alpha = Colorless.GetComponent<SpriteRenderer>();

            while (alpha.color.a >= 0)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a - DeltaAlpha);
                alpha.color = fadeColor;
            }
        }

        private IEnumerator BrighteningSprite()
        {
            Material colored = _renderer.material;
            float fade = colored.GetFloat(FadeValue);
            while (fade <= 1)
            {
                yield return new WaitForFixedUpdate();
                colored.SetFloat(FadeValue, fade += DeltaFade);
            }
        }

        private void ColoredSetup()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (!_renderer)
            {
                Debug.LogError("Doesn't have SpriteRenderer");
                return;
            }
            _renderer.material = PaintingService.Colored;
            _renderer.material.SetFloat(FadeValue, 0);
        }

        private void ColorlessSetup()
        {
            Colorless = Instantiate(gameObject, transform.position, transform.rotation, transform);
            _colorlessRenderer = Colorless.GetComponent<SpriteRenderer>();
            Paintable colorlessPaintable = Colorless.GetComponent<Paintable>();
            
            colorlessPaintable.SetColorless(Colorless);
            PaintingService.SetColorless(colorlessPaintable);
            
            _colorlessRenderer.material = PaintingService.Colorless;
            _colorlessRenderer.color = Color.gray;
        }
    }
}