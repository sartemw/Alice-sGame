using CodeBase.Infrastructure;
using CodeBase.Services.Repainting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace CodeBase.Fish
{
    public class ColoredFish : Colored
    {
        
        private IFishDataService _fishData;
        private IRepaintingService _repaintingService;
        private bool _flag = true;

        [Inject]
        public void Construct(IFishDataService fishData, IRepaintingService repaintingService)
        {
            _fishData = fishData;
            _repaintingService = repaintingService;
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