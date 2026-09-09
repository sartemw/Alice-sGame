using System;
using System.Collections;
using _Project.CodeBase.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.CodeBase.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] private HeroMove _heroMove;
    [SerializeField] public Animator _animator;

    private static readonly int MoveHash = Animator.StringToHash("Walking");
    private static readonly int IdleHash = Animator.StringToHash("IdleCount");
    private static readonly int AttackHash = Animator.StringToHash("AttackNormal");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int FlyHash = Animator.StringToHash("Fly");
    private static readonly int StartFlyHash = Animator.StringToHash("StartFly");
    private static readonly int StopFlyHash = Animator.StringToHash("StopFly");

    private readonly int _idleStateHash = Animator.StringToHash("Idles");
    private readonly int _startFlyStateHash = Animator.StringToHash("StartFly");
    private readonly int _stopFlyStateHash = Animator.StringToHash("StopFly");
    private readonly int _attackStateHash = Animator.StringToHash("Attack Normal");
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    private void Start()
    {
      StateEntered += OnStateEntered;
      StateExited += OnStateExited;
    }

    private void OnEnable()
    {
      StateEntered -= OnStateEntered;
      StateExited -= OnStateExited;
    }

    private void OnStateEntered(AnimatorState state)
    {
      if (state == AnimatorState.StartFly)
      {
        StartPoooof();
      }
      if (state == AnimatorState.StopFly)
      {
        StopPoooof();
      }
      if (state == AnimatorState.Idle)
      {
        StartCoroutine(SetIdleCount());
      }
    }

    private void OnStateExited(AnimatorState state)
    {
      if (state == AnimatorState.Idle)
      {
        StopCoroutine(SetIdleCount());
      }
    }

    public IEnumerator SetIdleCount()
    {
      while (true)
      {
        _animator.SetFloat(IdleHash, Random.Range(0, 3));
        yield return new WaitForSeconds(1);
      }
    }

    public AnimatorState State { get; private set; }
    public bool IsAttacking
    {
      get { return State == AnimatorState.Attack; }
    }
    
    public void Update()
    {
      _animator.SetFloat(MoveHash, Math.Abs(_heroMove.MovementVector.x));
    }

    public void PlayHit() => 
      _animator.SetTrigger(HitHash);

    public void PlayAttack() => 
      _animator.SetTrigger(AttackHash);

    public void PlayDeath() => 
      _animator.SetTrigger(DieHash);

    public void PlayFly() => 
      _animator.SetTrigger(FlyHash);
    
    public void PlayStartFly() => 
      _animator.SetTrigger(StartFlyHash);
    
    public void PlayStopFly() => 
      _animator.SetTrigger(StopFlyHash);
    
    public void ResetToIdle() => 
      _animator.Play(_idleStateHash, -1);

    public void StopPoooof()
    {
      Debug.Log("StopPoooof");
    }

    public void StartPoooof()
    {
      Debug.Log("StartPoooof");
    }
    public void EnteredState(int stateHash)
    {
      State = StateFor(stateHash);
      StateEntered?.Invoke(State);
    }

    public void ExitedState(int stateHash)
    {
      StateExited?.Invoke(StateFor(stateHash));
    }

    private AnimatorState StateFor(int stateHash)
    {
      AnimatorState state;
      if (stateHash == _idleStateHash)
      {
        state = AnimatorState.Idle;
      }
      else if (stateHash == _attackStateHash)
      {
        state = AnimatorState.Attack;
      }
      else if (stateHash == _walkingStateHash)
      {
        state = AnimatorState.Walking;
      }
      else if (stateHash == _deathStateHash)
      {
        state = AnimatorState.Died;
      }
      else if (stateHash == _startFlyStateHash)
      {
        state = AnimatorState.StartFly;
      }
      else if (stateHash == _stopFlyStateHash)
      {
        state = AnimatorState.StopFly;
      }
      else
      {
        state = AnimatorState.Unknown;
      }

      return state;
    }
  }
}