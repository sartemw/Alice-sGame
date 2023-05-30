using CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class ColoredFish : Colored
    {
        private IRepaintingService _repainting;
        private bool _flag = true;

        [Inject]
        public void Construct(IRepaintingService repainting)
        {
            _repainting = repainting;
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
                _repainting.FishPickUp(this);
                
                _flag = false;
            }
        }

       
        public class Factory : PlaceholderFactory<ColoredFish>
        {
        }
    }
}