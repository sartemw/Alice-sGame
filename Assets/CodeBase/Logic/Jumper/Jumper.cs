using System;
using CodeBase.Enemy;
using CodeBase.Hero;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.Jumper
{
    public class Jumper : MonoBehaviour
    {
        public TriggerObserver PointA;
        public TriggerObserver PointB;

        public Button ButtonA;
        public Button ButtonB;

        public float JumpForce;
        
        private Collider2D _jumpObject;
        private float _gravityScale;
        private HeroMove _heroMove;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            PointA.TriggerEnter += PrepareJumpA;
            PointB.TriggerEnter += PrepareJumpB;
            
            PointA.TriggerExit += ResetA;
            PointB.TriggerExit += ResetB;
        }

        private void Jump(Vector2 end)
        {
            _heroMove = _jumpObject.GetComponent<HeroMove>();
            _rigidbody2D = _jumpObject.GetComponent<Rigidbody2D>();
            _gravityScale = _rigidbody2D.gravityScale;

            _heroMove.FlipHero(new Vector2(end.x - _jumpObject.transform.position.x  , 0).normalized);
            _heroMove.enabled = false;
            _rigidbody2D.gravityScale = 0;
            
            _rigidbody2D
                .DOJump(end, JumpForce, 1, 2)
                .SetEase(Ease.InOutBack)
                .OnComplete(ActiveHero);
        }

        public void JumpA()
        {
            Vector2 end = PointB.transform.position;

            Jump(end);
        }

        public void JumpB()
        {
            Vector2 end = PointA.transform.position;
            
            Jump(end);
        }

        private void ActiveHero()
        {
            _heroMove.enabled = true;
            _rigidbody2D.gravityScale = _gravityScale;

        }

        private void PrepareJumpA(Collider2D obj)
        {
            _jumpObject = obj;
            ButtonA.gameObject.SetActive(true);
        }

        private void PrepareJumpB(Collider2D obj)
        {
            _jumpObject = obj;
            ButtonB.gameObject.SetActive(true);
        }

        private void ResetA(Collider2D obj)
        {
            _jumpObject = null;
            ButtonA.gameObject.SetActive(false);
        }

        private void ResetB(Collider2D obj)
        {
            _jumpObject = null;
            ButtonB.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            PointA.TriggerEnter -= PrepareJumpA;
            PointB.TriggerEnter -= PrepareJumpB;
            
            PointA.TriggerExit -= ResetA;
            PointB.TriggerExit -= ResetB;
        }
    }
}