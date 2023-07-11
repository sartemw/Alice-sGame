using System;
using CodeBase.Enemy;
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

        public GameObject[] Borders;
        
        public float JumpForce;
        
        private Collider2D _jumpObject;

        private void Start()
        {
            PointA.TriggerEnter += PrepareJumpA;
            PointB.TriggerEnter += PrepareJumpB;
            
            PointA.TriggerExit += ResetA;
            PointB.TriggerExit += ResetB;
        }

        private void Jump(Vector2 end)
        {
            ActiveBorders();
            _jumpObject.GetComponent<Rigidbody2D>()
                .DOJump(end, JumpForce, 1, 2)
                .SetEase(Ease.OutQuart)
                .OnComplete(ActiveBorders);
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

        private void ActiveBorders()
        {
            foreach (GameObject border in Borders)
            {
                border.SetActive(!border.activeSelf);
            }
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