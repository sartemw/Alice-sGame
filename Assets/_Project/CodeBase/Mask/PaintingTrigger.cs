using _Project.CodeBase.Enemy;
using _Project.CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Mask
{
    public class PaintingTrigger: MonoBehaviour
    {
        public TriggerObserver Trigger;

        [Inject]
        public void Construct(IPaintingService VARIABLE)
        {
            
        }
        private void Start()
        {
            Trigger.TriggerEnter += Enter;
        }

        private void Enter(Collider2D obj)
        {
            
        }
    }
}