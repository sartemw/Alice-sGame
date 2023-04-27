using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CodeBase.Fish
{
    public class Repaintable: MonoBehaviour
    {
        public ColorType ColorType;

        public void Painting(Material material)
        {
            if (gameObject.GetComponent<SpriteRenderer>())
                gameObject.GetComponent<SpriteRenderer>().material = material;
            if (gameObject.GetComponent<TilemapRenderer>())
                gameObject.GetComponent<TilemapRenderer>().material = material;
        }        
    }
}