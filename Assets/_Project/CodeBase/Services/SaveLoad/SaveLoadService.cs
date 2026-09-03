using System.Collections.Generic;
using _Project.CodeBase.Data;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Services.PersistentConfig;
using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace _Project.CodeBase.Services.SaveLoad
{
  public class SaveLoadService : ISaveLoadService
  {
    private const string ProgressKey = "Progress";
    private const string ConfigKey = "Config";

    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentConfigService _config;
    private readonly List<ISavedConfigReader> _savedConfigReaders = new List<ISavedConfigReader>();

    public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory, IPersistentConfigService config)
    {
      _progressService = progressService;
      _gameFactory = gameFactory;
      _config = config;
      _savedConfigReaders = config.ConfigReaders;
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
    
    public void SaveConfig()
    {
      foreach (ISavedConfig configWriter in _savedConfigReaders)
        configWriter.UpdateConfig(_config.ConfigData);
      
      PlayerPrefs.SetString(ConfigKey, _config.ConfigData.ToJson());
    }

    public ConfigData LoadConfig()
    {
      return PlayerPrefs.GetString(ConfigKey)?
        .ToDeserialized<ConfigData>();
    }
    
  }
}