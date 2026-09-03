using System;
using System.Collections;
using _Project.CodeBase.Data;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Logic;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Hero
{
  public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
  { 
    public HeroAnimator Animator; 
    private State _state;
    private IGameStateMachine _stateMachine;
    private IAudioService _audioService;

    public event Action HealthChanged;

    public void Construct(IGameStateMachine stateMachine, IAudioService audioService)
    {
      _audioService = audioService;
      _stateMachine = stateMachine;
    }

    public float Current
    {
      get => _state.CurrentHP;
      set
      {
        if (value != _state.CurrentHP)
        {
          _state.CurrentHP = value;
          
          HealthChanged?.Invoke();
        }
      }
    }

    public float Max
     {
       get => _state.MaxHP;
       set => _state.MaxHP = value;
     }


    public void LoadProgress(PlayerProgress progress)
     {
       _state = progress.HeroState;
       HealthChanged?.Invoke();
     }

     public void UpdateProgress(PlayerProgress progress)
     {
       progress.HeroState.CurrentHP = Current;
       progress.HeroState.MaxHP = Max;
     }

     public void TakeDamage(float damage)
     {
       if(Current <= 0)
         return;
       
       Current -= damage;
       Animator.PlayHit();
     }

     public void OutOfBorders()
     {
       _audioService.PlayLoseLevel();
       _stateMachine.Enter<RestartLevelState, string>(SceneManager.GetActiveScene().name);
     }
  }
}