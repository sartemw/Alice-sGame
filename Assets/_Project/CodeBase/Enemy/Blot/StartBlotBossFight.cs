using System;
using System.Collections.Generic;
using _Project.CodeBase.Enemy.Base;
using _Project.CodeBase.Events;
using _Project.CodeBase.StaticData.Windows;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class StartBossFight : Follow
    {
        private BaseOnEvent<AllPaintableInitializeSignal>  _allPaintableInitializeSignal  = new BaseOnEvent<AllPaintableInitializeSignal>();

        public MovingMarkerStaticData[] _movingPoints;
        
        private Tween _tween;
        private BossState _bossState;
        
        private Transform _heroTransform;
        private MovingMarkerStaticData _startPoint;
        private MovingMarkerStaticData _endPoint;
        
        private BossStates _currentState;
        
        

        public void Construct(Transform heroTransform, float speed, List<MovingMarkerStaticData> points)
        {
            Speed = speed;
            _heroTransform = heroTransform;
            _bossState = GetComponent<BossState>();
            
            _bossState.OnStateEntered += OnStateEntered;
            _bossState.OnStateExited += OnStateExited;
            
            SetupPoints(points);
        }

        private void OnStateEntered(BossStates obj)
        {
            if (obj == BossStates.StartFight)
            {
                _currentState = obj;
                StartFight();
            }
        }

        private void OnStateExited(BossStates obj)
        {
            
        }

        public override void MoveToTarget(Vector2 position)
        {
            _tween = transform
                .DOJump(position, 1.2f, 2, Speed)
                .OnComplete(TweenCompleted);
        }

        private void TweenCompleted()
        {
            _bossState.ExitedState(_currentState);
        }

        private void SetupPoints(List<MovingMarkerStaticData> points)
        {
            _movingPoints = new MovingMarkerStaticData[points.Count];
            
            foreach (MovingMarkerStaticData point in points)
            {
                switch (point.PointTypeId)
                {
                    case PointTypeId.StartPosition:
                        _startPoint = point;
                        _movingPoints[0] = point;
                        break;
                    case PointTypeId.EndPosition:
                        _endPoint = point;
                        _movingPoints[points.Count - 1] = point;
                        break;
                    case PointTypeId.Position_1:
                        _movingPoints[1] = point;
                        break;
                    case PointTypeId.Position_2:
                        _movingPoints[2] = point;
                        break;
                    case PointTypeId.Position_3:
                        _movingPoints[3] = point;
                        break;
                    case PointTypeId.Position_4:
                        _movingPoints[4] = point;
                        break;
                    case PointTypeId.Position_5:
                        _movingPoints[5] = point;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void StartFight() => 
            MoveToTarget(_startPoint.Position);

        private void OnEnable()
        {
            _bossState.OnStateEntered -= OnStateEntered;
            _bossState.OnStateExited -= OnStateExited;
            _tween.Kill();
        }
    }
}