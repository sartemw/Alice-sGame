using _Project.CodeBase.Data;

namespace _Project.CodeBase.Services.PersistentProgress
{
  public interface ISavedProgress : ISavedProgressReader
  {
    void UpdateProgress(PlayerProgress progress);
  }
}