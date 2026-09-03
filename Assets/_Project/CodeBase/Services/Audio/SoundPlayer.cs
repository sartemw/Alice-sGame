using _Project.CodeBase.Events;
using Ami.BroAudio;
using UnityEngine;

namespace _Project.CodeBase.Services.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundID _soundID;   
        
        public void PlaySound() =>
            EventBus.Invoke(new PlaySoundSignal
                {
                    Sound = _soundID
                }
            );
    }
}