using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Animations
{
    public class UIAnimator : MonoBehaviour
    {
        public enum AnimationType
        {
            Scale,
            Rotation,
            MoveHorizontal,
            MoveVertical,
            Fade
        }
        
        [Serializable]
        public struct UIAnimationStruct
        {
            public AnimationType Type;

            [Range(0,1)]
            public float Value;
        }

        public UIAnimationStruct[] Animations;
        
        private Sequence _sequence;
        private List<Tween> _tweens = new List<Tween>();

        private void Start()
        {
            foreach (UIAnimationStruct animation in Animations)
            {
                switch (animation.Type)
                {
                    case AnimationType.Scale:
                        Scale(animation.Value);
                        break;
                    case AnimationType.Rotation:
                        Rotation(animation.Value);
                        break;
                    case AnimationType.MoveHorizontal:
                        MoveHorizontal(animation.Value);
                        break;
                    case AnimationType.MoveVertical:
                        MoveVertical(animation.Value);
                        break;
                    case AnimationType.Fade:
                        Fade(animation.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Fade(float value)
        {
            var tween = GetComponent<Image>().DOFade(value, 1f).SetLoops(-1, LoopType.Yoyo);
            _tweens.Add(tween);
        }

        private void MoveVertical(float value)
        {
            value *= 100f;
            
            Vector3 moveDown = new Vector3(transform.position.x, transform.position.y - value, transform.position.z);
            Vector3 moveUp = new Vector3(transform.position.x, transform.position.y  + value, transform.position.z);
            Vector3 moveBase = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            _sequence = DOTween.Sequence();
            
            var tween = _sequence
                .Append(transform.DOMove(moveUp, 1f).SetEase(Ease.Linear))
                .Append(transform.DOMove(moveDown, 2f).SetEase(Ease.Linear))
                .Append(transform.DOMove(moveBase, 1f).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
            
            _tweens.Add(tween);
        }

        private void MoveHorizontal(float value)
        {
            value *= 100f;
            
            Vector3 moveLeft = new Vector3(transform.position.x - value, transform.position.y, transform.position.z);
            Vector3 moveRight = new Vector3(transform.position.x + value, transform.position.y, transform.position.z);
            Vector3 moveBase = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            var tween = _sequence = DOTween.Sequence();
            
            _sequence
                .Append(transform.DOMove(moveLeft, 0.75f).SetEase(Ease.Linear))
                .Append(transform.DOMove(moveRight, 1.5f).SetEase(Ease.Linear))
                .Append(transform.DOMove(moveBase, 0.75f).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
            
            _tweens.Add(tween);
        }

        private void Rotation(float value)
        {
            Vector3 rotateLeft = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + (-360 * value));
            Vector3 rotateRight = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + (360 * value));
            Vector3 rotateBase = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            
            var tween = _sequence = DOTween.Sequence();
            
            _sequence
                .Append(transform.DORotate(rotateLeft, 1f).SetEase(Ease.Linear))
                .Append(transform.DORotate(rotateRight, 2f).SetEase(Ease.Linear))
                .Append(transform.DORotate(rotateBase, 1f).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
            
            _tweens.Add(tween);
        }

        private void Scale(float value)
        {
            Vector3 scale = transform.localScale * value;
            var tween = transform.DOScale(scale, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            _tweens.Add(tween);
        }

        private void OnDestroy()
        {
            foreach (Tween tween in _tweens) 
                tween.Kill();
            
            _tweens.Clear();
        }
    }
}