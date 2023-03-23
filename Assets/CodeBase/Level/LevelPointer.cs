using System;
using System.Collections;
using CodeBase.Logic;
using CodeBase.Logic.PooledObjects;
using UnityEngine;

namespace CodeBase.Level
{
    public class LevelPointer : MonoBehaviour
    {
        public Pooler pooler;
        public SpriteRenderer InsidePointer;
        public SpriteRenderer OutsidePointer;
        public bool IsShining;
        [SerializeField] private float _speed;
        [SerializeField] private float _interval;

        
        private void Start()
        {
            InsidePointer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            OutsidePointer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            StartCoroutine(Shining());
        }

        private IEnumerator Shining()
        {
            float lenght = InsidePointer.size.y * transform.localScale.x;
            
            while (IsShining)
            {
                yield return new WaitForSeconds(_interval);
                StartCoroutine(StartMask(StartPoint(lenght), lenght));
            }
        }

        private IEnumerator StartMask(Vector2 startPoint, float lenght)
        {
            GameObject mask = CreateMask(startPoint);

            Vector2 endPoint = startPoint + new Vector2(
                transform.right.x * lenght * 1.2f,
                transform.right.y * lenght * 1.2f);
            while (CurrentDistance(mask, endPoint).sqrMagnitude > 0.1f)
            {
                yield return null;
                MoveMask(mask);
                
                if(CurrentDistance(mask, endPoint).sqrMagnitude > lenght*10)
                    break;
            }

            mask.SetActive(false);
        }

        private GameObject CreateMask(Vector2 startPoint)
        {
            GameObject mask = pooler.GetPooledObject();
            if (mask != null)
            {
                mask.transform.position = startPoint;
                mask.transform.rotation = transform.rotation;
                mask.SetActive(true);
            }

            return mask;
        }
        
        private Vector2 StartPoint(float lenght) => 
            transform.position + -transform.right * lenght/2;

        private static Vector2 CurrentDistance(GameObject mask, Vector2 endPoint) =>
            new Vector2(
                mask.transform.position.x - endPoint.x,
                mask.transform.position.y - endPoint.y);

        private void MoveMask(GameObject mask) => 
            mask.transform.Translate(Vector2.right * _speed);
    }
}