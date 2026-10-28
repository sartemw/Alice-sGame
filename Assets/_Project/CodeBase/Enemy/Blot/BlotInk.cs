using System;
using _Project.CodeBase.Data;
using _Project.CodeBase.Hero;
using _Project.CodeBase.Infrastructure.Effects;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Project.CodeBase.Enemy.Blot
{
    public class BlotInk : MonoBehaviour
    {
        public GameObject Sparks;
        public float Speed;

        private IAudioService _audioService;
        private IGameStateMachine _stateMachine;
        private HeroHealth _heroHealth;

        public void Construct(IAudioService audioService, IGameStateMachine stateMachine, HeroHealth heroHealth)
        {
            _audioService = audioService;
            _stateMachine = stateMachine;
            _heroHealth = heroHealth;
            
            CreateSparks();

            MoveTo();
        }

        private void MoveTo() => 
            transform.Translate(Vector2.down * Time.deltaTime * Speed);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Death();
                _heroHealth.TakeDamage(1000);
                
                DOVirtual.DelayedCall(2f, () =>
                    _stateMachine.Enter<RestartLevelState, string>(SceneManager.GetActiveScene().name));
            }
            if (other.CompareTag("Ground"))
            {
                Death();
            }
        }

        private void CreateSparks() => 
            Instantiate(Sparks, transform.position, transform.rotation);
        
        private void Death()
        {
            _audioService.PlayBlobs();
            CreateSparks();
            Destroy(gameObject);
        }
    }
}