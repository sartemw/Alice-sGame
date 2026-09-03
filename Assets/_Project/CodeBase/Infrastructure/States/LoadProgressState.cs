using _Project.CodeBase.Data;
using _Project.CodeBase.Services.PersistentConfig;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.SaveLoad;
using UnityEngine.Localization.Settings;
using Zenject;

namespace _Project.CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string InitialLevel = "MainMenu";
        private const string InitialLevel2 = "Cs_1";
        
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        private readonly IPersistentConfigService _configService;

        public LoadProgressState(GameStateMachine stateMachine, DiContainer diContainer)
        {
            _stateMachine = stateMachine;
            _progressService = diContainer.Resolve<IPersistentProgressService>();
            _saveLoadProgress = diContainer.Resolve<ISaveLoadService>();
            _configService = diContainer.Resolve<IPersistentConfigService>();
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            _stateMachine.Enter<LoadMainMenuState, string>(InitialLevel);
            //_stateMachine.Enter<LoadCutsceneState, string>(InitialLevel2);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _configService.ConfigData = 
                _saveLoadProgress.LoadConfig()
                ?? NewConfig();

            InformConfigReaders();
            
            _progressService.PlayerProgress = 
                _saveLoadProgress.LoadProgress() 
                ?? NewProgress();
        }

        private void InformConfigReaders()
        {
            foreach (ISavedConfigReader configReader in _configService.ConfigReaders)
                configReader.LoadConfig(_configService.ConfigData);
        }
        
        private ConfigData NewConfig()
        {
            var config = new ConfigData();
            config.Sound = true;
            config.Language = LocalizationSettings.SelectedLocale.Identifier.ToString();
            return config;
        }

        private PlayerProgress NewProgress()
        {
            var progress =  new PlayerProgress(initialLevel: "0-1");

            progress.HeroState.MaxHP = 50;
            progress.HeroStats.Damage = 1;
            progress.HeroStats.DamageRadius = 0.5f;
            progress.HeroState.ResetHP();
            progress.GameProgressData.CurrentLevel = 1;
      
            return progress;
        }

        // private string LoadLevel(int levelsCompleted) => 
        //     String.Format("{0}-{1}", levelsCompleted / 10, levelsCompleted % 10);
    }
}