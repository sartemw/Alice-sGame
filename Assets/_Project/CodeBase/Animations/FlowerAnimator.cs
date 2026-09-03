using _Project.CodeBase.Events;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Animations
{
    public class FlowerAnimator : EnvironmentAnimator
    {
        public Transform Petal;
       
        protected override void OnPlayActive(PaintingCompletedSignal obj)
        {
            if (_paintable.IsColored && obj.Target == _paintable)
            {
                GetComponent<SpriteRenderer>().material = _defaultMaterial;
                ChangeMaterialForAllChildren(_defaultMaterial);
                _animator.SetTrigger(ActiveHash);
                _tween = Petal.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}