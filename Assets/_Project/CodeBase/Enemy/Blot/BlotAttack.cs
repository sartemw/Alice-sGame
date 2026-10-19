using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class BlotAttack : Signatory
    {
        private BossState _bossState;
        private BossStats _bossStats;
        
        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();
            
            _bossState.OnStateEntered += OnStateEntered;
            _bossState.OnStateExited += OnStateExited;
        }

        private void OnStateEntered(BossStates obj)
        {
            if (obj == BossStates.Attack)
            {
                _bossState.CurrentState = obj;
                
                Attack();
            }
        }

        private void Attack()
        {
            Debug.Log("Attack");
            
            _bossState.ExitedState(_bossState.CurrentState);
        }

        private void OnStateExited(BossStates obj)
        {
            if (obj == BossStates.Attack) 
                _bossState.EnteredState(BossStates.Walking);
        }
        
        private void OnDisable()
        {
            _bossState.OnStateEntered -= OnStateEntered;
            _bossState.OnStateExited -= OnStateExited;
        }
    }
}