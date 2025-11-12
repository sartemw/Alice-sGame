using _Project.CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace _Project.CodeBase.UI.Windows
{
    public class CheatsWindow : WindowBase
    {
        [Header("Hero")]
        public float HeroSpeed;

        [Header("Level")] 
        public string Level;
        
        public void Construct(IPersistentProgressService progressService)
        {
            base.Construct(progressService);
        }
    
        protected override void Initialize()
        {
        }

        protected override void SubscribeUpdates()
        {
           // Progress.WorldData.LootData.Changed += RefreshSkullText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            //code
            //Progress.WorldData.LootData.Changed -= RefreshSkullText;
        }
    }
}