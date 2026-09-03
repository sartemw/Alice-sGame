using System.Collections;
using _Project.CodeBase.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.CodeBase.Services.Repainting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePaintable: Paintable
    {
        public SpriteRenderer[] ChildrenColorless;
        public SpriteRenderer[] ChildrenColored;
        
        private SpriteRenderer _renderer;
        private SpriteRenderer _colorlessRenderer;
        
        public override void Initialize()
        {
            ColoredSetup();
            ColorlessSetup();
        }

        public void SwitchMaterialInCutscene()
        {
            if (IsColored)
            {
                _renderer.material = PaintingService.ColoredMaterial;
                ColorlessObject.GetComponent<SpriteRenderer>().material = PaintingService.ColorlessMaterial;
                foreach (SpriteRenderer spriteRenderer in ChildrenColored)
                {
                    spriteRenderer.material = PaintingService.ColoredMaterial;
                }              
                foreach (SpriteRenderer spriteRenderer in ChildrenColorless)
                {
                    spriteRenderer.material = PaintingService.ColorlessMaterial;
                } 
            }
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

                foreach (SpriteRenderer child in ChildrenColorless) 
                    child.color = fadeColor;
            }
        }

        private IEnumerator FadeSprite(float deltaAlpha)
        {
            Material colored = _renderer.material;
            float fade = colored.GetFloat(FadeValue);

            if (deltaAlpha == DeltaAlphaToFade)
                ColorlessObject.SetActive(!ColorlessObject.activeSelf);
            
            while (fade > 0)
            {
                yield return new WaitForFixedUpdate();
                colored.SetFloat(FadeValue, fade -= deltaAlpha);

                foreach (SpriteRenderer child in ChildrenColored) 
                    child.material.SetFloat(FadeValue, fade);
            }
            
            EventBus.Invoke(new FadingCompletedSignal(){Target = this});
        }

        private IEnumerator FadeAlphaSprite(float delta)
        {
            SpriteRenderer alpha = ColorlessObject.GetComponent<SpriteRenderer>();

            while (alpha.color.a >= 0)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a - delta);
                alpha.color = fadeColor;
                
                foreach (SpriteRenderer child in ChildrenColorless) 
                    child.color = fadeColor;
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
                
                foreach (SpriteRenderer child in ChildrenColored) 
                    child.material.SetFloat(FadeValue, fade);
            }
            
            if (delta == 1)
                ColorlessObject.SetActive(!ColorlessObject.activeSelf);
            
            EventBus.Invoke(new PaintingCompletedSignal(){Target = this});
        }

        private void ColoredSetup()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (!_renderer)
            {
                Debug.LogError("Doesn't have SpriteRenderer");
                return;
            }
            ChildrenColored = GetComponentsInChildren<SpriteRenderer>();

            _renderer.material = PaintingService.ColoredMaterial;
            _renderer.material.SetFloat(FadeValue, 0);

            foreach (SpriteRenderer spriteRenderer in ChildrenColored)
            {
                spriteRenderer.material = PaintingService.ColoredMaterial;
                spriteRenderer.material.SetFloat(FadeValue, 0);
            }
        }

        private void ColorlessSetup()
        {
            ColorlessObject = Instantiate(gameObject, transform.position, transform.rotation, transform);
            ChildrenColorless = ColorlessObject.GetComponentsInChildren<SpriteRenderer>();
            _colorlessRenderer = ColorlessObject.GetComponent<SpriteRenderer>();
            
            SwitchMaterialAndColor(_colorlessRenderer, ColorlessObject);
            
            foreach (SpriteRenderer spriteRenderer in ChildrenColorless)
            {
                spriteRenderer.material = PaintingService.ColorlessMaterial;
                spriteRenderer.sortingOrder += 1;
                spriteRenderer.color = Color.gray;
            }
        }
    }
}