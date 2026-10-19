using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class StartBlotBossFight : Signatory
    {
        private Tween _tween;
        private BossState _bossState;
        private BossStats _bossStats;

        private Vector2 _startPoint;
        private float _speed;
        
        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();
            
            _speed = _bossStats.Speed;
            _startPoint = _bossStats.StartPoint.Position;
            
            _bossState.OnStateEntered += OnStateEntered;
            _bossState.OnStateExited += OnStateExited;
        }
        
        private void OnStateEntered(BossStates obj)
        {
            if (obj == BossStates.StartFight)
            {
                _bossState.CurrentState = obj;
                StartFight();
            }
        }
        private void OnStateExited(BossStates obj)
        {
            if (obj == BossStates.StartFight) 
                _bossState.EnteredState(BossStates.Walking);
        }

        private void StartFight() => 
            GetComponent<WalkingBlotBoss>().MoveToTarget(_startPoint);

        private void OnDisable()
        {
            _bossState.OnStateEntered -= OnStateEntered;
            _bossState.OnStateExited -= OnStateExited;
            _tween.Kill();
        }
    }
}