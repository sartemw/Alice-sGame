using System;
using System.Collections;
using CodeBase.Services.Repainting;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Mask
{
    public class ScalerPaintingMask : MonoBehaviour
    {
        public event Action OnScalingComplete; 
        public float Duration;


        private SpriteRenderer _spriteRenderer;
        private IRepaintingService _repaintingService;
        
        [Inject]
        public void Construct(IRepaintingService repaintingService)
        {
            _repaintingService = repaintingService;
        }
        public void StartScaling(float value) 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            DOTween.Sequence()
                .SetEase(Ease.OutQuad)
                .Append(transform.DOScale(value, Duration))
                .Append(_spriteRenderer.material.DOFade(0, 1))
                .OnComplete(ScalingCompleted);
        }


        private void ScalingCompleted()
        {
            OnScalingComplete?.Invoke();
        }

        private static bool StopScaling(float timeToExit) => 
            timeToExit > 0;

        public class Factory : PlaceholderFactory<ScalerPaintingMask>
        {
        }
    }
}