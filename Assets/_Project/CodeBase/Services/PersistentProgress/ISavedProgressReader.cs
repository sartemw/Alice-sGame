using _Project.CodeBase.Data;

namespace _Project.CodeBase.Services.PersistentProgress
{
  public interface ISavedProgressReader
  {
    void LoadProgress(PlayerProgress progress);
  }
}