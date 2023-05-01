using CodeBase.Fish;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CodeBase.Services.Repainting
{
    [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapRepaintable: Repaintable
    {
        public void Painting(Material material)
        {
            TilemapRenderer renderer = gameObject.GetComponent<TilemapRenderer>();
            
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}