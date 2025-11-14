using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Jumper
{
    public class Hopper : MonoBehaviour
    {
        private SpriteRenderer _sprite;
        private Vector2 _pointA, _pointB, _pointChanged;
        private float _jumpForce;
        private bool _jumpFlag;
        public void HopperInstantiate(Vector2 pointA, Vector2 pointB, float jumpForce)
        {
            _pointA = pointA;
            _pointB = pointB;
            _jumpForce = jumpForce;

            _sprite = GetComponent<SpriteRenderer>();

            Flip();
        }

        private void HopperJump()
        {
            transform
                .DOJump(ChangePoint(), _jumpForce, 1, 2)
                .SetEase(Ease.OutExpo)
                .OnComplete(Flip);
        }

        private Vector3 ChangePoint()
        {
            _jumpFlag = !_jumpFlag;

            switch (_jumpFlag)
            {
                
                case true:
                    _pointChanged = _pointB;
                    break;

                case false:
                    _pointChanged = _pointA;
                    break;
            }
            
            return _pointChanged;
        }

        private void Flip()
        {
            transform
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
            transform
                .DOMoveY(transform.position.y - 0.5f, 0.7f)
                .OnComplete(HopperJump);
        }
    }
}