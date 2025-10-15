using _Project.CodeBase.Events;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Animations
{
    public class ObjectAnimator : MonoBehaviour
    {
        public Vector2[] MovementPoints;
        
        private int _iterator = 0;

        public void AllLevelColoring() =>
            EventBus.Invoke(new AllLevelColoringSignal());

        public void FadeMaterial() => 
            EventBus.Invoke(new FadeMaterialSignal());

        public void TranslateTo()
        {
            if (_iterator == MovementPoints.Length)
                ToNextScene();
            
            gameObject.transform.DOMove(MovementPoints[_iterator], 2);
            _iterator++;
        }

        private void ToNextScene()
        {
            _iterator = 0;
        }
    }
}