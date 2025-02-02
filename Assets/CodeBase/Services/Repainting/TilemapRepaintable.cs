using System;
using CodeBase.Data;
using CodeBase.Fish;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace CodeBase.Services.Repainting
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapRepaintable: Repaintable
    {
        private IRepaintingService _repaintingService;
        [Inject]
        public void Construct(IRepaintingService repaintingService)
        {
            _repaintingService = repaintingService;
            ColoredSetup();
            ColorlessSetup();
        }

        private void ColoredSetup()
        {
            TilemapRenderer coloredRenderer = GetComponent<TilemapRenderer>();
            if (!coloredRenderer)
            {
                Debug.LogError("Doesn't have TilemapRenderer"); 
                return;
            }
            //coloredRenderer.sortingOrder = -(int) ColorType*10;
            coloredRenderer.sortingOrder = ColorType.SorterPaintLayer()*10;
            coloredRenderer.material = _repaintingService.Colored;
            coloredRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        private void ColorlessSetup()
        {
            GameObject colorless = Instantiate(gameObject, transform.position, transform.rotation, transform);
            TilemapRenderer colorlessRenderer = colorless.GetComponent<TilemapRenderer>();
            //colorlessRenderer.sortingOrder = -(int) ColorType*10;
            colorlessRenderer.sortingOrder = ColorType.SorterPaintLayer()*10;
            colorlessRenderer.material = _repaintingService.Colorless;
            colorlessRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        public new void Painting(Material material)
        {
            base.Painting(material);
        }
    }
}