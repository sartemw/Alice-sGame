﻿using System.Collections;
using CodeBase.Data;
using CodeBase.Services.FishCollectorService;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class ColoredFish : MonoBehaviour
    {
        public ColorType FishColorType;
        public Color FishColor;
        //public Sprite[] FishAtlas;

        private int _i;
        private IRepaintingService _repainting;

        [Inject]
        public void Construct(IRepaintingService repainting)
        {
            _repainting = repainting;
            //FishAtlas = fishsAtlas;
        }

        private void Start()
        {
            if (FishColorType == ColorType.Rainbow)
            {
               StartCoroutine(RainbowColor());
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _repainting.AddFish(this);
        }

        #region RainbowColor

        private void OnEnable() => 
            StopCoroutine(RainbowColor());

        private IEnumerator RainbowColor()
        {
            float checkTime = Time.time;
            SpriteRenderer fishSprite = gameObject.GetComponent<SpriteRenderer>();
            Color firstColor = ChangeColor();
            Color secondColor = ChangeColor();
            
            while (true)
            {
                if ( Time.time - checkTime > 1)
                {
                    firstColor = secondColor;
                    secondColor = ChangeColor();
                    
                    checkTime = Time.time;
                }

                FishColor = Color.Lerp(firstColor, secondColor,  Time.time  - checkTime );
                yield return new WaitForSeconds(0.1f);
                fishSprite.color = FishColor;
            }
        }

        private Color ChangeColor()
        {
            if (_i == 7) 
                _i = 0;
            
            var color = FishColorType.SwitchColor(_i);
            _i++;
            return color;
        }

        #endregion

        public class Factory : PlaceholderFactory<ColoredFish>
        {
        }
    }
}