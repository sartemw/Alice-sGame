using UnityEngine;

namespace _Project.CodeBase.Logic.Helpers.Jumper
{
    public class HopperAnimator : MonoBehaviour
    { 
        private static readonly int PrepareHash = Animator.StringToHash("Prepare");
        private static readonly int JumpUpHash = Animator.StringToHash("JumpUp");
        private static readonly int JumpDownHash = Animator.StringToHash("JumpDown");
        

        private Animator _animator;

        public void Init(Animator animator) => 
            _animator = animator;

        public void PlayPrepare() => 
            _animator.SetTrigger(PrepareHash);

        public void PlayJumpUp() => 
            _animator.SetTrigger(JumpUpHash);

        public void PlayJumpDown() => 
            _animator.SetTrigger(JumpDownHash);
    }
}