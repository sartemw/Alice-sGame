using _Project.CodeBase.StaticData.Windows;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class WalkingBlotBoss : Signatory
    {
        private Tween _tween;
        private BossState _bossState;
        private BossStats _bossStats;

        private MovingMarkerStaticData[] _walkingPoints;
        private int _currentPoint;
        private float _speed;
        
        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();
            
            _speed = _bossStats.Speed;
            _currentPoint = 0;
            
            _walkingPoints = new MovingMarkerStaticData[_bossStats.MovingPoints.Length];
            _walkingPoints = _bossStats.MovingPoints;
            
            _bossState.OnStateEntered += OnStateEntered;
            _bossState.OnStateExited += OnStateExited;
        }

        public void MoveToTarget(Vector2 position) =>
            _tween = transform
                .DOJump(position, 1.2f, 2, _speed)
                .SetEase(Ease.InOutSine)
                .OnComplete(TweenCompleted);

        private void OnStateEntered(BossStates obj)
        {
            if (obj == BossStates.Walking)
            {
                _bossState.CurrentState = obj;
                FindWalkingPoint();
            }
        }

        private void OnStateExited(BossStates obj)
        {
            if (obj == BossStates.Walking) 
                _bossState.EnteredState(BossStates.FindTarget);
        }

        private void FindWalkingPoint()
        {
            int i = _currentPoint;
            while (_currentPoint == i)
            {
                i = Random.Range(1, _walkingPoints.Length - 1);
            }
            _currentPoint = i;
            
            MoveToTarget(_walkingPoints[_currentPoint].Position);
        }

        private void TweenCompleted()
        {
            _tween.Kill();
            _bossState.ExitedState(_bossState.CurrentState);
        }

        private void OnDisable()
        {
            _bossState.OnStateEntered -= OnStateEntered;
            _bossState.OnStateExited -= OnStateExited;
            _tween.Kill();
        }
    }
}