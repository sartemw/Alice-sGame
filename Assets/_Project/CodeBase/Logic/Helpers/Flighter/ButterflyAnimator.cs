using UnityEngine;

namespace _Project.CodeBase.Logic.Helpers.Flighter
{
    public class ButterflyAnimator : MonoBehaviour
    { 
        private static readonly int FlyHash = Animator.StringToHash("Fly");
        
        private Animator _animator;

        public void Init(Animator animator) => 
            _animator = animator;

        public void PlayFly() => 
            _animator.SetTrigger(FlyHash);

    }
}