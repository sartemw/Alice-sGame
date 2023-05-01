using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.Repainting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRepaintable: Repaintable
    {
        public void Painting(Material material)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}