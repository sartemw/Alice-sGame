using UnityEngine;

namespace CodeBase.Services.Audio
{
    public class AudioService: IAudioService
    {
        private AudioSource _audioSource;
        private IAudioAssetsService _audioAssets;

        public AudioService(AudioSource audioSource, IAudioAssetsService audioAssets)
        {
            _audioSource = audioSource;
            _audioAssets = audioAssets;
        }

        public void PlayRandomBlob() => 
            PlaySound(_audioAssets.ForRandomBlobs());

        private void PlaySound(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            
            _audioSource.Play();
        }
    }
}