using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string InitialLevel = "0-1";
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
      
            _gameStateMachine.Enter<LoadLevelState, string>(LoadLevel(CurrentLevelProgress()));
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
                level = 1;
            return level;
        }

        private string LoadLevel(int levelsCompleted) => 
            String.Format("{0}-{1}", levelsCompleted / 10, levelsCompleted % 10);
    }
}