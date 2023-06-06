using System;
using CodeBase.Fish;

namespace CodeBase.Services.Repainting
{
    public interface IFishDataService: IService
    {
        public event Action<ColoredFish> FishPickedUp;
        public int FishOnLevel { get; set; }
        public void FishPickUp(ColoredFish fish);
        public void Restart();
    }
}