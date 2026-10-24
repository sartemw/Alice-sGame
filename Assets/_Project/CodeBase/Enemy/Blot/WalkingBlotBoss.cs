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
        private int _pointNumber;
        private float _speed;
        
        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();
            
            _speed = _bossStats.Speed;
            _pointNumber = 0;

            _walkingPoints = new MovingMarkerStaticData[_bossStats.MovingPoints.Length];
            _walkingPoints = _bossStats.MovingPoints;
            
            _bossStats.CurrentPoint = _walkingPoints[_pointNumber];

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
            int i;

            do {
                i = Random.Range(0, _walkingPoints.Length - 1);
            } while (_bossStats.CurrentPoint == _walkingPoints[i]);
           
            _pointNumber = i;
            _bossStats.CurrentPoint = _walkingPoints[_pointNumber];
            
            MoveToTarget(_walkingPoints[_pointNumber].Position);
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