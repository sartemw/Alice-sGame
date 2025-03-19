using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.CodeBase.Services.Repainting
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapPaintable: Paintable
    {
        private TilemapRenderer _renderer;
        
        private TilemapRenderer _colorlessRenderer;
       
        public override void Initialize()
        {
            ColoredSetup();
            ColorlessSetup();
        }
     

        public override void Fade() => 
            StartCoroutine(FadeTilemap());

        public override void Brightening() => 
            StartCoroutine(BrighteningTilemap());

        private IEnumerator FadeTilemap()
        {
            Tilemap alpha = Colorless.GetComponent<Tilemap>();

            while (alpha.color.a >= 0)
            {
                yield return new WaitForFixedUpdate();
                Color fadeColor = new Color(alpha.color.r, alpha.color.g, alpha.color.b, alpha.color.a - DeltaAlpha);
                alpha.color = fadeColor;
            }
        }

        private IEnumerator BrighteningTilemap()
        {
            Material colored = _renderer.material;
            float fade = colored.GetFloat("_Fade");
            while (fade <= 1)
            {
                yield return new WaitForFixedUpdate();
                colored.SetFloat("_Fade", fade += DeltaFade);
            }
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
            _renderer.material.SetFloat("_Fade", 0);
        }

        private void ColorlessSetup()
        {
            Colorless = Instantiate(gameObject, transform.position, transform.rotation, transform);
            _colorlessRenderer = Colorless.GetComponent<TilemapRenderer>();
            Paintable colorlessPaintable = Colorless.GetComponent<Paintable>();
            
            colorlessPaintable.SetColorless(Colorless);
            PaintingService.SetColorless(colorlessPaintable);
            
            _colorlessRenderer.material = PaintingService.Colorless;
            _colorlessRenderer.material.color = Color.grey;
        }
    }
}