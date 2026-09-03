using _Project.CodeBase.Data;
using _Project.CodeBase.Services.PersistentConfig;

namespace _Project.CodeBase.Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    PlayerProgress LoadProgress();

    public void SaveConfig();

    public ConfigData LoadConfig();
  }
}