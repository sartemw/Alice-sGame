using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string InitialLevel = "MainMenu";
        
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        
        public LoadProgressState(GameStateMachine stateMachine, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _progressService = diContainer.Resolve<IPersistentProgressService>();
            _saveLoadProgress = diContainer.Resolve<ISaveLoadService>();
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            //_stateMachine.Enter<LoadLevelState, string>("0-1");
            _stateMachine.Enter<LoadMainMenuState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
            
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = 
                _saveLoadProgress.LoadProgress() 
                ?? NewProgress();
            
            CurrentLevelProgress();
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

        private int CurrentLevelProgress()
        {
            int level = _progressService.Progress.GameProgressData.LevelsCompleted = _saveLoadProgress.LoadLevelCompleted();
            if (level == 0)
                level = _progressService.Progress.GameProgressData.LevelsCompleted = 1;
            
            return level;
        }

        private string LoadLevel(int levelsCompleted) => 
            String.Format("{0}-{1}", levelsCompleted / 10, levelsCompleted % 10);
    }
}