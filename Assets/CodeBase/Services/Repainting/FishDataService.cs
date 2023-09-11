using System;
using CodeBase.Fish;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.Repainting
{
    public class FishDataService : IFishDataService
    {
        private readonly IStaticDataService _staticDataService;
        public event Action<ColoredFish> FishPickedUp;
        public int FishOnLevel { get; set; }
        
        public FishDataService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        public void FishPickUp(ColoredFish fish)
        {
            if (FishOnLevel > 0)
                FishOnLevel--;
            FishPickedUp?.Invoke(fish);
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