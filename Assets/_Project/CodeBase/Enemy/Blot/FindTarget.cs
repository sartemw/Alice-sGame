using _Project.CodeBase.StaticData.Windows;
using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class FindTarget : Signatory
    {
        private BossState _bossState;
        private BossStats _bossStats;
        
        private Transform _target;
        private int _iterator;
        
        private MovingMarkerStaticData[] _walkingPoints;
        private MovingMarkerStaticData _nearestPoint;

        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();
            
            _target = _bossStats.HeroTransform;
            
            _walkingPoints = new MovingMarkerStaticData[_bossStats.MovingPoints.Length];
            _walkingPoints = _bossStats.MovingPoints;
            
            _bossState.OnStateEntered += OnStateEntered;
            _bossState.OnStateExited += OnStateExited;
        }

        private void OnStateEntered(BossStates obj)
        {
            if (obj == BossStates.FindTarget)
            {
                _bossState.CurrentState = obj;
                FindTargetPoint();
            }
        }

        private void FindTargetPoint()
        {
            float distance = Mathf.Infinity;
            float positionX = _target.position.x;

            for (int i = 0; i < _walkingPoints.Length - 1; i++)
            {
                float currentDistance = Mathf.Abs(_walkingPoints[i].Position.x - positionX);

                if (currentDistance < distance)
                {
                    _nearestPoint = _walkingPoints[i];
                    distance = currentDistance;
                }
            }

            _bossStats.CurrentPoint = _nearestPoint;
            GetComponent<WalkingBlotBoss>().MoveToTarget(_nearestPoint.Position);
        }

        private void OnStateExited(BossStates obj)
        {
            if (obj == BossStates.FindTarget) 
                _bossState.EnteredState(BossStates.Attack);
        }
        
        private void OnDisable()
        {
            _bossState.OnStateEntered -= OnStateEntered;
            _bossState.OnStateExited -= OnStateExited;
        }
    }
}