using _Project.CodeBase.Hero;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.UI.Windows.Cheats;
using UnityEngine;

namespace _Project.CodeBase.UI.Windows
{
    public class CheatsWindow : WindowBase
    {
        private IStaticDataService _staticDataService;
        private GameObject _hero;

        public CheatPanel Money;
        public CheatPanel HeroSpeed;
        public CheatPanel LevelOpen;

        public new void Construct(IPersistentProgressService progressService, IStaticDataService staticDataService, GameObject hero)
        {
            base.Construct(progressService);
            
            _staticDataService = staticDataService;
            _hero = hero;
            
            Money.OnValueChanged += OnMoneyChanged;
            HeroSpeed.OnValueChanged += OnHeroSpeedChanged;
            LevelOpen.OnValueChanged += OnLevelOpenChanged;
        }

        private void OnLevelOpenChanged(float value) => 
            Progress.GameProgressData.CurrentLevel = (int)value;

        private void OnHeroSpeedChanged(float value) => 
            _hero.GetComponent<HeroMove>().MovementSpeed = value;

        private void OnMoneyChanged(float value) => 
            Debug.Log("Money changed to " + value);


        private void OnDestroy()
        {
            Money.OnValueChanged -= OnMoneyChanged;
            HeroSpeed.OnValueChanged -= OnHeroSpeedChanged;
            LevelOpen.OnValueChanged -= OnLevelOpenChanged;
        }
    
        protected override void Initialize()
        {
            Money.SetValue(Progress.WorldData.LootData.Collected);
            HeroSpeed.SetValue(_staticDataService.ForHero(HeroTypeId.Cat).MoveSpeed);
            LevelOpen.SetValue(Progress.GameProgressData.CurrentLevel);
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