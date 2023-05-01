using System.Collections;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class ScalerPaintingMask : MonoBehaviour
    {
        public float ScalingTime = 3;
        public float IncreaseScale;

        public void StartScaling() => 
            StartCoroutine(Scaling());

        private IEnumerator Scaling()
        {
            Vector3 startScale = transform.localScale;
            Vector3 endScale = transform.localScale * IncreaseScale;
            float startTime = 0;
            float endTime = ScalingTime;
            float timeToExit = ScalingTime;
            
            while (StopScaling(timeToExit))
            {
                timeToExit -= Time.deltaTime;
                startTime += Time.deltaTime;
                yield return null;
                transform.localScale = Vector3.Lerp(startScale, endScale, startTime/endTime);
            }
        }

        private static bool StopScaling(float timeToExit) => 
            timeToExit > 0;

        public class Factory : PlaceholderFactory<ScalerPaintingMask>
        {
        }
    }
}