using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace _Project.CodeBase.Tutorials
{
    public class Paw : MonoBehaviour
    {
        public GameObject Ring;
        public Transform ClickTransform;
        
        private const string ClickAnimation = "Click";
        private const string HideAnimation = "Paw_hide";
        private Animator _animator;
        private SpriteSkin _spriteSkin;
        private int _iterator = 0;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteSkin = GetComponent<SpriteSkin>();
        }

        public void Click() => 
            _animator.Play(ClickAnimation);
        
        public void CreateRing()
        {
            _iterator++;
            GameObject ring = Instantiate(Ring, ClickTransform.position, ClickTransform.rotation);

            Destroy(ring, 2f);

            if (_iterator > 5)
                Stop();
        }

        private void Stop()
        {
            _animator.enabled = false;
            
            PawHide();
        }

        private void PawHide() =>
            gameObject.transform.DOMoveY(gameObject.transform.position.y + 10, 5);
    }
}