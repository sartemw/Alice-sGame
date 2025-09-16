using Ami.BroAudio;

namespace _Project.CodeBase.Services.Audio
{
    public interface IAudioService: IService
    {
        void Init();
        void PlayBlobs();
        void PlayOpenWindow();
    }
}