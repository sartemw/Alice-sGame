using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.Audio
{
    public class AudioAssetsService : IAudioAssetsService
    {
        private const string BlobsDataPath = "Audio/Blobs";

        private Dictionary<int, AudioClip> _blobs;

        public void Load()
        {
            int i = 0;
            _blobs = Resources
                .LoadAll<AudioClip>(BlobsDataPath)
                .ToDictionary(x => i++, x => x);
        }
        
        public AudioClip ForBlobs(int typeId) =>
            _blobs.TryGetValue(typeId, out AudioClip staticData)
                ? staticData
                : null;

        public AudioClip ForRandomBlobs() =>
            _blobs.TryGetValue(Random.Range(0, _blobs.Count), out AudioClip staticData)
                ? staticData
                : null;
    }
}