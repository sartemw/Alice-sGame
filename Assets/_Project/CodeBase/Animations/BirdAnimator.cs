using _Project.CodeBase.Events;
using UnityEngine;

namespace _Project.CodeBase.Animations
{
    public class BirdAnimator : EnvironmentAnimator
    {
        protected override void OnPlayActive(PaintingCompletedSignal obj)
        {
            if (_paintable.IsColored && obj.Target == _paintable)
            {
                GetComponent<SpriteRenderer>().material = _defaultMaterial;
                ChangeMaterialForAllChildren(_defaultMaterial);
                _animator.SetTrigger(ActiveHash);
            }
        }
    }
}