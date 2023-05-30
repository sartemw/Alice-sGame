using System.Collections;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Fish
{
    public class Colored: MonoBehaviour
    {
        public ColorType ColorType;
        public Color Color;

        private int _i;

        private void OnEnable() => 
            StopCoroutine(RainbowColor());

        public IEnumerator RainbowColor()
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

                Color = Color.Lerp(firstColor, secondColor,  Time.time  - checkTime );
                yield return new WaitForSeconds(0.1f);
                fishSprite.color = Color;
            }
        }

        private Color ChangeColor()
        {
            if (_i == 7) 
                _i = 0;
            
            var color = ColorType.SwitchColor(_i);
            _i++;
            return color;
        }

    }
}