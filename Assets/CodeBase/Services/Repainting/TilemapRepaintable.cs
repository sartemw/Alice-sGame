using System;
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
       
        private void ColorlessSetup()
        {
            GameObject colorless = Instantiate(gameObject, transform.position, transform.rotation, transform);
            TilemapRenderer colorlessRenderer = colorless.GetComponent<TilemapRenderer>();
            colorlessRenderer.sortingOrder = -(int) ColorType*10;
            colorlessRenderer.material = _repaintingService.Colorless;
            colorlessRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        private void ColoredSetup()
        {
            TilemapRenderer coloredRenderer = GetComponent<TilemapRenderer>();
            if (!coloredRenderer) return;
            coloredRenderer.sortingOrder = -(int) ColorType*10;
            coloredRenderer.material = _repaintingService.Colored;
            coloredRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public new void Painting(Material material)
        {
            base.Painting(material);
        }
    }
}