using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.EventBus.Events;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Fish
{
    public class ColoredFish : Colored
    {
        private IFishDataService _fishData;
        private IPaintingService _paintingService;
        private bool _flag = true;

        [Inject]
        public void Construct(IFishDataService fishData, IPaintingService paintingService)
        {
            _fishData = fishData;
            _paintingService = paintingService;
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
                Position = transform.position;
                EventBus.EventBus.Invoke(new FishPickupSignal
                {
                    ColoredFish = this
                });
                
                _flag = false;
            }
        }

        public class Factory : PlaceholderFactory<ColoredFish>
        {
            
        }
    }
}