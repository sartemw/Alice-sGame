using System.Collections;
using _Project.CodeBase.Enemy;
using _Project.CodeBase.Hero;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Jumper
{
    public class Jumper : MonoBehaviour
    {
        public TriggerObserver PointA;
        public TriggerObserver PointB;

        public ClickTrigger ButtonA;
        public ClickTrigger ButtonB;

        public Vector2 PositionPointA;
        public Vector2 PositionPointB;
        public Vector2 HopperPointA;
        public Vector2 HopperPointB;
        
        public float JumpForce;
        public bool FlagCanJumpB;
        
        public Hopper Hopper;

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

            ButtonA.TriggeredClick += JumpA;
            ButtonB.TriggeredClick += JumpB;

            PointA.transform.position = PositionPointA;
            PointB.transform.position = PositionPointB;
            Hopper.HopperInstantiate(HopperPointA, HopperPointB, JumpForce);
        }

        private void Jump(Vector2 end)
        {
            _heroMove = _jumpObject.GetComponent<HeroMove>();
            _rigidbody2D = _jumpObject.GetComponent<Rigidbody2D>();
            _gravityScale = _rigidbody2D.gravityScale;
            
            _heroMove.FlipHero(new Vector2(end.x - _jumpObject.transform.position.x  , 0).normalized);
            _heroMove.enabled = false;
            _rigidbody2D.gravityScale = 0;
            
            StartCoroutine(ActiveHeroCor());
            
            _jumpObject.transform
                .DOJump(end, JumpForce, 1, 2)
                .SetEase(Ease.OutExpo)
                .OnComplete(ActiveHero);
        }

        private IEnumerator ActiveHeroCor()
        {
            yield return new WaitForSeconds(3);
            _rigidbody2D.gravityScale = 15;
        }

        private void JumpA() => 
            Jump(PointB.transform.position);

        private void JumpB() => 
            Jump(PointA.transform.position);

        private void ActiveHero()
        {
            _heroMove.enabled = true;
            _rigidbody2D.gravityScale = 15;
        }

        private void PrepareJumpA(Collider2D obj)
        {
            _jumpObject = obj;
            ButtonA.gameObject.SetActive(true);
        }

        private void PrepareJumpB(Collider2D obj)
        {
            if (!FlagCanJumpB)
                return;
            
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
            
            ButtonA.TriggeredClick -= JumpA;
            ButtonB.TriggeredClick -= JumpB;
        }
    }
}