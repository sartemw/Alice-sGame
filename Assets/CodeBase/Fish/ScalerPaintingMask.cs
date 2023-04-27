using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class ScalerPaintingMask : MonoBehaviour
    {
        public float ScalingTime;
        public int IncreaseScale;
        
        public void Start()
        {
            StartCoroutine(StartScaling());
        }

        private IEnumerator StartScaling()
        {
            Vector3 startScale = transform.localScale;
            Vector3 endScale = transform.localScale * IncreaseScale;
            float startTime = 0;
            float endTime = ScalingTime;
            float timeToExit = ScalingTime;
            
            while (timeToExit > 0)
            {
                timeToExit -= Time.deltaTime;
                startTime += Time.deltaTime;
                yield return null;
                transform.localScale = Vector3.Lerp(startScale, endScale, startTime/endTime);
            }
        }
        public class Factory : PlaceholderFactory<ScalerPaintingMask>
        {
        }
    }
}