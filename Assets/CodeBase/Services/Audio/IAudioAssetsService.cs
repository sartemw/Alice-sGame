using UnityEngine;

namespace CodeBase.Services.Audio
{
    public interface IAudioAssetsService: IService
    {
        public void Load();
        public AudioClip ForBlobs(int typeId);
        public AudioClip ForRandomBlobs();
    }
}