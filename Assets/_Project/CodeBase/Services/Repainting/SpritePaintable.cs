using System.Collections;
using _Project.CodeBase.Events;
using UnityEngine;

namespace _Project.CodeBase.Services.Repainting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePaintable: Paintable
    {
        private SpriteRenderer _renderer;
        
        private SpriteRenderer _colorlessRenderer;
        
        public override void Initialize()
        {
            ColoredSetup();
            ColorlessSetup();
        }

        protected override void Brightening(StartPaintingSignal obj)
        {
            if (obj.Target == this)
            {
                StartCoroutine(BrighteningSprite(DeltaAlphaToBright));
                StartCoroutine(FadeAlphaSprite(DeltaFadeToBright));                
            }
        }

        protected override void BrighteningInstantly(StartPaintingInstantlySignal obj)
        {
            if (obj.Target == this)
            {
                StartCoroutine(BrighteningSprite(1));
                StartCoroutine(FadeAlphaSprite(1));
            }
        }

        protected override void Fade(FadeMaterialSignal obj)
        {
            if (obj.Target == this)
            {
                StopCoroutine(BrighteningSprite(1));
                StopCoroutine(FadeAlphaSprite(1));
                
                StartCoroutine(FadeSprite(DeltaAlphaToFade));
                StartCoroutine(UpAlphaSprite(DeltaFadeToFade));
            }
        }

        private IEnumerator UpAlphaSprite(float deltaFade)
        {
            SpriteRenderer alpha = ColorlessObject.GetComponent<SpriteRenderer>();

            while (alpha.color.a < 1)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a + deltaFade);
                alpha.color = fadeColor;
            }
        }

        private IEnumerator FadeSprite(float deltaAlpha)
        {
            Material colored = _renderer.material;
            float fade = colored.GetFloat(FadeValue);
            while (fade > 0)
            {
                yield return new WaitForFixedUpdate();
                colored.SetFloat(FadeValue, fade -= deltaAlpha);
            }
            
            EventBus.Invoke(new PaintingCompletedSignal());
        }

        private IEnumerator FadeAlphaSprite(float delta)
        {
            SpriteRenderer alpha = ColorlessObject.GetComponent<SpriteRenderer>();

            while (alpha.color.a >= 0)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a - delta);
                alpha.color = fadeColor;
            }
        }

        private IEnumerator BrighteningSprite(float delta)
        {
            Material colored = _renderer.material;
            float fade = colored.GetFloat(FadeValue);
            while (fade <= 1)
            {
                yield return new WaitForFixedUpdate();
                colored.SetFloat(FadeValue, fade += delta);
            }
            
            EventBus.Invoke(new PaintingCompletedSignal());
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
            ColorlessObject = Instantiate(gameObject, transform.position, transform.rotation, transform);
            _colorlessRenderer = ColorlessObject.GetComponent<SpriteRenderer>();
            
            SwitchMaterialAndColor(_colorlessRenderer, ColorlessObject);

            // foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            //     SwitchMaterialAndColor(spriteRenderer, spriteRenderer.gameObject);
        }
    }
}