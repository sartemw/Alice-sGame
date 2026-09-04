using _Project.CodeBase.Enemy;
using _Project.CodeBase.Hero;
using _Project.CodeBase.Services.Audio;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Logic.Helpers.Flighter
{
    public class Flighter : MonoBehaviour
    {
        public TriggerObserver PointA;
        public TriggerObserver PointB;

        public ClickTrigger ButtonA;
        public ClickTrigger ButtonB;

        public Vector2 PositionPointA;
        public Vector2 PositionPointB;
        public Vector2 ButterflyPointA;
        public Vector2 ButterflyPointB;

        public int WavesCount;
        public float WaveHeight;
        public float Duration;

        public bool FlagCanFlyB;

        public Butterfly Butterfly;

        private Collider2D _flyObject;
        private HeroMove _heroMove;
        private Rigidbody2D _rigidbody2D;
        private bool _canFly = true;
        private Tween _tween;
        private SoundPlayer _soundPlayer;

        private void Start()
        {
            _soundPlayer = GetComponent<SoundPlayer>();
            
            PointA.TriggerEnter += PrepareFlyA;
            PointB.TriggerEnter += PrepareFlyB;

            PointA.TriggerExit += ResetA;
            PointB.TriggerExit += ResetB;

            ButtonA.TriggeredClick += FlyA;
            ButtonB.TriggeredClick += FlyB;

            PointA.transform.position = PositionPointA;
            PointB.transform.position = PositionPointB;
            Butterfly.ButterflyInitialize(ButterflyPointA, ButterflyPointB, this);
        }

        private void Fly(Vector2 end)
        {
            if (!_canFly)
                return;
            
            _soundPlayer.PlaySound();
            
            _canFly = false;
            _heroMove = _flyObject.GetComponent<HeroMove>();
            _rigidbody2D = _flyObject.GetComponent<Rigidbody2D>();

            _heroMove.FlipHero(new Vector2(end.x - _flyObject.transform.position.x, 0).normalized);
            _heroMove.enabled = false;
            _rigidbody2D.gravityScale = 0;

            //поменять на движение по синусойде с появление крыльев бабочки
            _tween = FlyTween(end)
                .OnComplete(ActiveHero);
        }

        private Sequence FlyTween(Vector2 end)
        {
            int segments = WavesCount * 4; // 4 сегмента на волну (вверх, вниз, вверх, вниз)
            Sequence sequence = DOTween.Sequence();

            Vector2 pointA = _flyObject.transform.position;
            Vector2 pointB = end;
            
            for (int i = 0; i < segments; i++)
            {
                float nextT = (float)(i + 1) / segments;
            
                // Вычисляем позиции для текущего и следующего сегмента
                Vector2 nextPos = GetSinePosition(nextT, pointA, pointB, WaveHeight, WavesCount);
            
                sequence.Append(_flyObject.transform
                        .DOMove(nextPos, Duration / segments)
                        .SetEase(Ease.Linear));
            }
            return sequence;
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
        
        private void FlyA() =>
            Fly(PointB.transform.position);

        private void FlyB() =>
            Fly(PointA.transform.position);

        private void ActiveHero()
        {
            _heroMove.enabled = true;
            _rigidbody2D.gravityScale = 15;
            _canFly = true;
        }

        private void PrepareFlyA(Collider2D obj)
        {
            _flyObject = obj;
            ButtonA.gameObject.SetActive(true);
        }

        private void PrepareFlyB(Collider2D obj)
        {
            if (!FlagCanFlyB)
                return;

            _flyObject = obj;
            ButtonB.gameObject.SetActive(true);
        }

        private void ResetA(Collider2D obj)
        {
            _flyObject = null;
            ButtonA.gameObject.SetActive(false);
        }

        private void ResetB(Collider2D obj)
        {
            _flyObject = null;
            ButtonB.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PointA.TriggerEnter -= PrepareFlyA;
            PointB.TriggerEnter -= PrepareFlyB;

            PointA.TriggerExit -= ResetA;
            PointB.TriggerExit -= ResetB;

            ButtonA.TriggeredClick -= FlyA;
            ButtonB.TriggeredClick -= FlyB;
            
            _tween.Kill();
        }
    }
}