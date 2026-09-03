using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Repainting;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Animations
{
    public abstract class EnvironmentAnimator : MonoBehaviour
    {
        private readonly BaseOnEvent<PaintingCompletedSignal>  _onPaintingCompleted  = new BaseOnEvent<PaintingCompletedSignal>();
        protected abstract void OnPlayActive(PaintingCompletedSignal obj);
        
        protected static readonly int ActiveHash = Animator.StringToHash("Active");
        
        protected Material _defaultMaterial;
        protected SpritePaintable _paintable;
        protected Animator _animator;
        protected Tween _tween;

        [SerializeField] bool _isActive = false;
        protected void Awake() => 
            _defaultMaterial = GetComponent<SpriteRenderer>().material;

        protected void Start()
        {
            _paintable = GetComponent<SpritePaintable>();
            _animator = GetComponent<Animator>();
            
            EventBus.Subscribe(_onPaintingCompleted.SetOnInvoke(OnPlayActive));
            
            
            if (_isActive)
                OnPlayActive();
        }

        private void OnPlayActive() => 
            _animator.SetTrigger(ActiveHash);
        
        protected void OnDestroy() => 
            _tween.Kill();
        
        protected void ChangeMaterialForAllChildren(Material material)
        {
            SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer child in children) 
                child.material = material;
        }
    }
}