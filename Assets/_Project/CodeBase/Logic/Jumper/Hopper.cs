using _Project.CodeBase.Events;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Jumper
{
    public class Hopper : MonoBehaviour
    {
        private BaseOnEvent<LevelTransferTriggerEnter>  _onLevelTransferTriggerEnter  = new BaseOnEvent<LevelTransferTriggerEnter>();

        private SpriteRenderer _sprite;
        public Vector2 PointA;
        public Vector2 PointB;
        private float _jumpForce;
        private bool _jumpFlag;
        public void HopperInstantiate(Vector2 pointA, Vector2 pointB, float jumpForce)
        {
            gameObject.transform.position = pointA;
            PointA = pointA;
            PointB = pointB;
            _jumpForce = jumpForce;

            _sprite = GetComponent<SpriteRenderer>();
            Flip();
            
            EventBus.Subscribe(_onLevelTransferTriggerEnter.SetOnInvoke(Stop));
        }

        private void Stop(LevelTransferTriggerEnter obj)
        {
            transform.DOKill();
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

        private void HopperJump()
        {
            transform
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
    }
}