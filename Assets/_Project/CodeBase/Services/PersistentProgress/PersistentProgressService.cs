using _Project.CodeBase.Data;

namespace _Project.CodeBase.Services.PersistentProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    public PlayerProgress PlayerProgress { get; set; }
  }
}