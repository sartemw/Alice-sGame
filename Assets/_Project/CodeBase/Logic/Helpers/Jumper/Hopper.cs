using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Helpers.Jumper
{
    public class Hopper : MonoBehaviour
    { 
        private SpriteRenderer _sprite;
        public Vector2 PointA;
        public Vector2 PointB;
        private float _jumpForce;
        private bool _jumpFlag;
        private Tween _tween;

        private HopperAnimator _hopperAnimator;
        
        private float _currentHeight;
        private bool _isMoveUp;
        public void HopperInitialize(Vector2 pointA, Vector2 pointB, float jumpForce)
        {
            gameObject.transform.position = pointA;
            PointA = pointA;
            PointB = pointB;
            _jumpForce = jumpForce;

            _sprite = GetComponent<SpriteRenderer>();
            _hopperAnimator = GetComponent<HopperAnimator>();
            _hopperAnimator.Init(GetComponent<Animator>());
            _currentHeight = transform.position.y;

            Flip();
        }

        private void OnDestroy() => 
            _tween.Kill();

        private void Update() => 
            DetectingHeight();

        private void Flip()
        {
            _hopperAnimator.PlayPrepare();
            
            _tween = transform
                .DOMoveY(transform.position.y + 0.5f, 1)
                .SetEase(Ease.InExpo)
                .OnComplete(() => 
                { 
                    _sprite.flipX = !_sprite.flipX;
                    MoveDown();
                });
        }

        private void MoveDown()
        {
            _tween = transform
                .DOMoveY(transform.position.y - 0.5f, 0.7f)
                .OnComplete(HopperJump);
        }

        private void HopperJump()
        {
            _hopperAnimator.PlayPrepare();
            
            _tween = transform
                .DOJump(ChangePoint(), _jumpForce, 1, 1.5f)
                .SetEase(Ease.InSine)
                .OnComplete(Flip);
        }

        private Vector3 ChangePoint()
        {
            _jumpFlag = !_jumpFlag;
            Vector2 point;

            switch (_jumpFlag)
            {
                
                case true:
                    point = PointB;
                    break;

                case false:
                    point = PointA;
                    break;
            }
            
            return point;
        }

        private void DetectingHeight()
        {
            if (transform.position.y > _currentHeight)
            {
                _isMoveUp = true;
                _currentHeight = transform.position.y;
            }
            else if (_isMoveUp == true)
            {
                _isMoveUp = false;
                _currentHeight = transform.position.y;
                _hopperAnimator.PlayJumpDown();
            } 
            else 
            {
                _isMoveUp = false;
                _currentHeight = transform.position.y;
            }
        }
    }
}