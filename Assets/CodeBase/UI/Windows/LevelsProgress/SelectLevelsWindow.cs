using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.LevelsProgress
{
    public class SelectLevelsWindow : WindowBase
    {
        public GameObject LevelIcon;
        public RectTransform LevelsContainer;

        public void Construct(IPersistentProgressService progressService)
        {
            base.Construct(progressService);
        }
        
        protected override void Initialize() => 
            RefreshLevelsContainer();

        protected override void SubscribeUpdates() => 
            Progress.WorldData.LootData.Changed += RefreshLevelsContainer;

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.Changed -= RefreshLevelsContainer;
        }

        private void RefreshLevelsContainer()
        {
            int levelsCompleted = Progress.GameProgressData.LevelsCompleted;

            for (int i = 1; i < 16; i++)
            {
                GameObject level = Instantiate(LevelIcon, LevelsContainer);
                level.GetComponentInChildren<TMP_Text>().text = i.ToString();
                if (i > levelsCompleted)
                {
                    level.GetComponentInChildren<TMP_Text>().text = "X";
                }
            }
        }
    }
}