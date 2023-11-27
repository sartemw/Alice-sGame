using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    PlayerProgress LoadProgress();
    void SaveLevelCompleted();
    int LoadLevelCompleted();
  }
}