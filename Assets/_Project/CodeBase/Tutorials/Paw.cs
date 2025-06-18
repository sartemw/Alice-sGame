using UnityEngine;
using UnityEngine.U2D.Animation;

namespace _Project.CodeBase.Tutorials
{
    public class Paw : MonoBehaviour
    {
        public GameObject Ring;
        public Transform ClickTransform;
        
        private const string ClickAnimation = "Click";
        private Animator _animator;
        private SpriteSkin _spriteSkin;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteSkin = GetComponent<SpriteSkin>();
        }

        public void Click() => 
            _animator.Play(ClickAnimation);
        

        public void CreateRing()
        {
            GameObject ring = Instantiate(Ring, ClickTransform.position, ClickTransform.rotation, gameObject.transform);

            Destroy(ring, 2f);
        }
    }
}