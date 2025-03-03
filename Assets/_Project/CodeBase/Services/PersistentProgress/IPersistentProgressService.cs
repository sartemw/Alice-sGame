using _Project.CodeBase.Data;

namespace _Project.CodeBase.Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get; set; }
  }
}