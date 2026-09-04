using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Helpers.Flighter
{
    public class Butterfly : MonoBehaviour
    {
        [SerializeField] private Vector2 _pointA;
        [SerializeField] private Vector2 _pointB;
        [SerializeField] private float _duration;
        [SerializeField] private float _waveHeight; // высота волны
        [SerializeField] private int _wavesCount; // количество волн

        private bool _jumpFlag;
        private Tween _tween;
        private bool _isMoveUp;
        
        public void ButterflyInitialize(Vector2 pointA, Vector2 pointB, Flighter flighter)
        {
            gameObject.transform.position = pointA;
            _pointA = pointA;
            _pointB = pointB;
            _duration = flighter.Duration;
            _waveHeight = flighter.WaveHeight;
            _wavesCount = flighter.WavesCount;

            _tween?.Kill();
            transform.position = _pointA;
            
            _tween = Fly();
        }
        private void OnDestroy() => 
            _tween.Kill();

        private Sequence Fly()
        {
            int segments = _wavesCount * 4; // 4 сегмента на волну (вверх, вниз, вверх, вниз)
            Sequence sequence = DOTween.Sequence();

            Vector2 pointA = transform.position;
            Vector2 pointB = ChangePoint();
            
            for (int i = 0; i < segments; i++)
            {
                float nextT = (float)(i + 1) / segments;
            
                // Вычисляем позиции для текущего и следующего сегмента
                Vector2 nextPos = GetSinePosition(nextT, pointA, pointB, _waveHeight, _wavesCount);
            
                // Добавляем сегмент движения
                sequence.Append(transform
                        .DOMove(nextPos, _duration / segments)
                        .SetEase(Ease.Linear))
                    .OnComplete(() =>
                    {
                        StartCoroutine(FlipScale());
                        
                        Revert();
                    });
            }
            return sequence;
        }
        
        private IEnumerator FlipScale()
        {
            float t = 0;
            var startVector = transform.localScale;
            var toVector = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            while (t < 1)
            {
                yield return null;
                t += 0.01f;
                transform.localScale = Vector3.Lerp(startVector, toVector, t);
            }
        }
        
        private void Revert()
        {
            _tween?.Kill();
            Fly();
        }

        private Vector2 GetSinePosition(float t, Vector2 start, Vector2 end, float amplitude, int waves)
        {
            // Линейная интерполяция между точками
            Vector2 basePosition = Vector2.Lerp(start, end, t);
        
            // Добавляем синусоидальное смещение по Y
            float sineOffset = Mathf.Sin(t * waves * Mathf.PI * 2) * amplitude;
        
            // Смещаем перпендикулярно направлению движения
            Vector2 perpendicular = new Vector2(-(end - start).normalized.y, (end - start).normalized.x);
        
            return basePosition + perpendicular * sineOffset;
        }
        
        private Vector2 ChangePoint()
        {
            _jumpFlag = !_jumpFlag;
            Vector2 point;

            switch (_jumpFlag)
            {
                
                case true:
                    point = _pointB;
                    break;

                case false:
                    point = _pointA;
                    break;
            }
            
            return point;
        }
    }
}