using System;
using System.Collections.Generic;
using _Project.CodeBase.Data;
using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.SaveLoad;
using _Project.CodeBase.Services.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Logic
{
  public class LevelTransferTrigger : MonoBehaviour, ISavedProgress
  {
    private const string PlayerTag = "Player";
    private const string InitialLevel = "MainMenu";
    public string TransferTo;
    private IGameStateMachine _stateMachine;
    private bool _triggered;
    private ISaveLoadService _saveLoadService;
    private bool _isCompleted;

    public void Construct(IGameStateMachine stateMachine, ISaveLoadService saveLoadService)
    {
      _stateMachine = stateMachine;
      _saveLoadService = saveLoadService;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if(_triggered)
        return;

      EventBus.Invoke(new LevelTransferTriggerEnter());
      
      if (other.CompareTag(PlayerTag))
      {
        _saveLoadService.SaveProgress();
        
        if (_isCompleted)
          _stateMachine.Enter<LoadMainMenuState, string>(InitialLevel);
        else
          _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        
        _triggered = true;
      }
    }

    public void LoadProgress(PlayerProgress progress) => 
      _isCompleted = IsCompleted(progress);

    public void UpdateProgress(PlayerProgress progress)
    {
      List<string> completedLevelsList = progress.GameProgressData.CompletedLevels;
      
      if(!_isCompleted && !IsCompleted(progress))
      {
        completedLevelsList.Add(SceneManager.GetActiveScene().name);
        progress.GameProgressData.FirstTimeComplete();
      }
    }

    private bool IsCompleted(PlayerProgress progress) => 
      progress.GameProgressData.CompletedLevels.Contains(SceneManager.GetActiveScene().name);
  }
}
