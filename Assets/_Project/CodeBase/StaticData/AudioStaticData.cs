using Ami.BroAudio;
using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Static Data/Audio")]
    public class AudioStaticData : ScriptableObject
    {
        public SoundID MainMenu;
        public SoundID Game1;
        public SoundID Blobs;
        public SoundID OpenWindow;
    }
}