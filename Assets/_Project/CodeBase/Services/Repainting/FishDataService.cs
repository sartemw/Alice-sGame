using System;
using _Project.CodeBase.EventBus;
using _Project.CodeBase.EventBus.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Services.Repainting
{
    public class FishDataService : IFishDataService
    {
        private readonly IStaticDataService _staticDataService;
        
        private BaseOnEvent<FishPickupSignal>  _onFishPickup  = new BaseOnEvent<FishPickupSignal>();
        public int FishOnLevel { get; set; }
        
        public FishDataService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            
            EventBus.EventBus.Subscribe(_onFishPickup.SetOnInvoke(OnFishPickUp));
        }
        public void OnFishPickUp(FishPickupSignal fish)
        {
            if (FishOnLevel > 0)
                FishOnLevel--;
        }

        public void Restart()
        {
            if (!LevelStaticData()) return;
            
            if (LevelStaticData().FishSpawners.Count == 0)
                FishOnLevel = 0;
            else
                FishOnLevel =  LevelStaticData().FishSpawners.Count;
        }

        private LevelStaticData LevelStaticData() => 
            _staticDataService.ForLevel(SceneManager.GetActiveScene().name);
    }
}