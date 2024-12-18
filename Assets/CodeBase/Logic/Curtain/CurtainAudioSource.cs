using UnityEngine;

namespace CodeBase.Logic.Curtain
{
    public class CurtainAudioSource: MonoBehaviour
    {
        [SerializeField] private AudioClip[] _splash;
        
        private AudioSource _audioSource;
        
        
        private void Start() => 
            _audioSource = GetComponent<AudioSource>();

        public void SplashWater ()
        {
            _audioSource.clip = _splash[Random.Range(0,_splash.Length)];
            _audioSource.Play();
        }
    }
}