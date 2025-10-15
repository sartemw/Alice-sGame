using System.Collections;
using _Project.CodeBase.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.CodeBase.Services.Repainting
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapPaintable: Paintable
    {
        private const string FadeValue = "_Fade";
        private TilemapRenderer _renderer;
        
        private TilemapRenderer _colorlessRenderer;
       
        public override void Initialize()
        {
            ColoredSetup();
            ColorlessSetup();
        }

        protected override void Brightening(StartPaintingSignal obj)
        {
            if (obj.Target == this)
            {
                StartCoroutine(BrighteningTilemap(DeltaFade));
                StartCoroutine(FadeAlphaTilemap(DeltaAlpha));
            }
        }

        protected override void BrighteningInstantly(StartPaintingInstantlySignal obj)
        {
            if (obj.Target == this)
            {
                StartCoroutine(BrighteningTilemap(1));
                StartCoroutine(FadeAlphaTilemap(1));
            }
        }

        protected override void Fade(FadeMaterialSignal obj)
        {
            Material material = GetComponent<TilemapRenderer>().material;
            if (material.name == ColoredMaterial) 
                StartCoroutine(FadeMaterial(material));
        }

        private IEnumerator FadeAlphaTilemap(float delta)
        {
            Tilemap alpha = ColorlessObject.GetComponent<Tilemap>();

            while (alpha.color.a >= 0)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a - delta);
                alpha.color = fadeColor;
            }
        }
        
        private IEnumerator BrighteningTilemap(float delta)
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
            _renderer = GetComponent<TilemapRenderer>();
            if (!_renderer)
            {
                Debug.LogError("Doesn't have TilemapRenderer"); 
                return;
            }
            _renderer.material = PaintingService.Colored;
            _renderer.material.SetFloat(FadeValue, 0);
        }

        private void ColorlessSetup()
        {
            ColorlessObject = Instantiate(gameObject, transform.position, transform.rotation, transform);
            _colorlessRenderer = ColorlessObject.GetComponent<TilemapRenderer>();
            SwitchMaterialAndColor(_colorlessRenderer, ColorlessObject);
        }
    }
}