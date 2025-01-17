﻿using CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Repainting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRepaintable: Repaintable
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
            SpriteRenderer colorlessRenderer = colorless.GetComponent<SpriteRenderer>();
            colorlessRenderer.sortingOrder = -(int) ColorType*10;
            colorlessRenderer.material = _repaintingService.Colorless;
            colorlessRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        private void ColoredSetup()
        {
            SpriteRenderer coloredRenderer = GetComponent<SpriteRenderer>();
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