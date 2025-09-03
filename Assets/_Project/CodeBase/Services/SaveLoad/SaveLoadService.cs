using _Project.CodeBase.Data;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace _Project.CodeBase.Services.SaveLoad
{
  public class SaveLoadService : ISaveLoadService
  {
    private const string ProgressKey = "Progress";

    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;

    public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
    {
      _progressService = progressService;
      _gameFactory = gameFactory;
    }

    public void SaveProgress()
    {
      foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
        progressWriter.UpdateProgress(_progressService.PlayerProgress);
      
      PlayerPrefs.SetString(ProgressKey, _progressService.PlayerProgress.ToJson());
    }

    public PlayerProgress LoadProgress()
    {
      PlayerProgress progress = PlayerPrefs.GetString(ProgressKey)?
        .ToDeserialized<PlayerProgress>();
   
      return progress;
    }
  }
}