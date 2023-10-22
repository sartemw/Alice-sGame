using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    public string InitialLevel = "1-1";
    
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = 
        _saveLoadProgress.LoadProgress() 
        ?? NewProgress();
      
      _progressService.Progress.GameProgressData.LevelsCompleted = _saveLoadProgress.LoadLevelCompleted();
      if (_progressService.Progress.GameProgressData.LevelsCompleted == 0)
        _progressService.Progress.GameProgressData.LevelsCompleted = 1;
    }

    private PlayerProgress NewProgress()
    {
      var progress =  new PlayerProgress(initialLevel: InitialLevel);

      progress.HeroState.MaxHP = 50;
      progress.HeroStats.Damage = 1;
      progress.HeroStats.DamageRadius = 0.5f;
      progress.HeroState.ResetHP();
      progress.GameProgressData.LevelsCompleted = 1;
      
      return progress;
    }
  }
}