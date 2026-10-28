using _Project.CodeBase.Hero;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.Audio;
using UnityEngine;

namespace _Project.CodeBase.Enemy.Blot
{
    public class BlotAttack : Signatory
    {
        private BossState _bossState;
        private BossStats _bossStats;
        private IAudioService _audioService;
        private IGameStateMachine _stateMachine;
        private HeroHealth _heroHealth;

        public override void Construct()
        {
            _bossState = GetComponent<BossState>();
            _bossStats = GetComponent<BossStats>();

            _audioService = _bossStats.AudioService;
            _stateMachine = _bossStats.StateMachine;
            _heroHealth = _bossStats.HeroTransform.GetComponent<HeroHealth>();
            
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
            GameObject ink = Instantiate(_bossStats.BlotInk, transform.position, transform.rotation);
            ink.GetComponent<BlotInk>().Construct(_audioService, _stateMachine, _heroHealth);
            
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