using System;
using CodeBase.Enemy;
using CodeBase.Services.Repainting;
using UnityEngine;
using Zenject;

namespace CodeBase.Mask
{
    public class PaintingTrigger: MonoBehaviour
    {
        public TriggerObserver Trigger;

        [Inject]
        public void Construct(IRepaintingService VARIABLE)
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