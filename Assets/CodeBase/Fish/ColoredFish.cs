using CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class ColoredFish : Colored
    {
        
        private IFishDataService _fishData;
        private bool _flag = true;

        [Inject]
        public void Construct(IFishDataService fishData)
        {
            _fishData = fishData;
        }

        private void Start()
        {
            if (ColorType == ColorType.Rainbow)
            {
               StartCoroutine(RainbowColor());
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_flag)
            {

                _fishData.FishPickUp(this);
                
                _flag = false;
            }
        }

       
        public class Factory : PlaceholderFactory<ColoredFish>
        {
            
        }
    }
}