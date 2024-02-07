using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.LevelsProgress
{
    public class SelectLevelsWindow : WindowBase
    {
        public GameObject LevelIcon;
        public RectTransform LevelsContainer;
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine,
            SceneLoader sceneLoader)
        {
            base.Construct(progressService);
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        protected override void Initialize() => 
            RefreshLevelsContainer();

        protected override void SubscribeUpdates() => 
            Progress.GameProgressData.Changed += RefreshLevelsContainer;

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.GameProgressData.Changed -= RefreshLevelsContainer;
        }

        private void RefreshLevelsContainer()
        {
            int levelsCompleted = Progress.GameProgressData.LevelsCompleted;
            for (int j = 0; j < 1; j++)
            {
                for (int i = 1; i < 11; i++)
                {
                    GameObject level = Instantiate(LevelIcon, LevelsContainer);
                    level.GetComponentInChildren<TMP_Text>().text = i.ToString();
                    if (i > levelsCompleted)
                    {
                        level.GetComponentInChildren<TMP_Text>().text = "X";
                        level.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        LoadLevelButton loadLevel = level.GetComponent<LoadLevelButton>();
                        loadLevel.Init(_stateMachine, _sceneLoader);
                        loadLevel.LoadLevel = $"{j}-{i}";
                    }
                }
            }
        }
    }
}