using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    void SaveLevelCompleted();
    int LoadLevelCompleted();
    PlayerProgress LoadProgress();
  }
}