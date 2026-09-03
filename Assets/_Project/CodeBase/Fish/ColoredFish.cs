using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Fish
{
    public class ColoredFish : Colored
    {
        private IFishDataService _fishData;
        private IPaintingService _paintingService;
        private bool _flag = true;
        private IAudioService _audioService;

        [Inject]
        public void Construct(IFishDataService fishData, IPaintingService paintingService, IAudioService audioService)
        {
            _audioService = audioService;
            _fishData = fishData;
            _paintingService = paintingService;
        }

        private void Start()
        {
            base.Construct();
            
            if (ColorType == ColorType.Rainbow)
            {
               StartCoroutine(RainbowColor());
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_flag)
            {
                _audioService.PlayPickupFish();
                Position = transform.position;
                EventBus.Invoke(new FishPickupSignal
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