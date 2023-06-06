using System;
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
        
        public void StartScaling(float value) 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            DOTween.Sequence()
                .SetEase(Ease.OutQuad)
                .Append(transform.DOScale(value, Duration))
                .Append(_spriteRenderer.material.DOFade(0, 1))
                .OnComplete(ScalingCompleted);
        }


        private void ScalingCompleted() => 
            OnScalingComplete?.Invoke();

        public class Factory : PlaceholderFactory<ScalerPaintingMask>
        {
        }
    }
}