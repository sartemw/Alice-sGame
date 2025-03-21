using _Project.CodeBase.StaticData;
using UnityEngine;

namespace _Project.CodeBase.Services.Audio
{
    public class AudioService: IAudioService
    {
        private readonly ConfigStaticData _config;
        
        private AudioSource _audioSource;
        private IAudioAssetsService _audioAssets;

        public AudioService(AudioSource audioSource, IAudioAssetsService audioAssets, ConfigStaticData config)
        {
            _audioSource = audioSource;
            _audioAssets = audioAssets;
            _config = config;
        }

        public void PlayRandomBlob() => 
            PlaySound(_audioAssets.ForRandomBlobs());

        private void PlaySound(AudioClip audioClip)
        {
            if (!_config.Sound)
                return;
            
            _audioSource.clip = audioClip;
            
            _audioSource.Play();
        }
    }
}