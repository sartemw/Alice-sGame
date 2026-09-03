using System;
using _Project.CodeBase.Services.Repainting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.CodeBase.Logic.TilePlatforms
{
    public abstract class TilePlatform : MonoBehaviour
    {
        public Collider2D Collider;
        public TilemapRenderer Body;
        
        protected TilemapPaintable _paintable;

        private void Awake()
        {
            _paintable = Body.GetComponent<TilemapPaintable>();
        }
    }
}