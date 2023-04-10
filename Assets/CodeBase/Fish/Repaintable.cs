using System;
using UnityEngine;

namespace CodeBase.Fish
{
    public class Repaintable: MonoBehaviour
    {
        public Sprite Colored;
        public Sprite Colorless;
        public ColorType ColorType;

        public void PaintingColorless()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Colorless;
        }

        public void PaintingColored()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Colored;
        }        
    }
}