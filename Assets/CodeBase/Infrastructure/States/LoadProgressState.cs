using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    public string InitialLevel;
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IStaticDataService _staticDataService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress, IStaticDataService staticDataService)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
      _staticDataService = staticDataService;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      
      //_gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
      _gameStateMachine.Enter<LoadMainMenuState>();
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = 
        _saveLoadProgress.LoadProgress() 
        ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      var progress =  new PlayerProgress(initialLevel: InitialLevel);
      
      HeroStaticData heroData = _staticDataService.ForHero(HeroTypeId.Cat);
      
      progress.HeroState.MaxHP = heroData.Hp;
      progress.HeroStats.Damage = heroData.Damage;
      progress.HeroStats.DamageRadius = heroData.Cleavage;
      
      progress.HeroState.ResetHP();

      return progress;
    }
  }
}