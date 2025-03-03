using System;
using _Project.CodeBase.Fish;

namespace _Project.CodeBase.Services.Repainting
{
    public interface IFishDataService: IService
    {
        public event Action<ColoredFish> FishPickedUp;
        public int FishOnLevel { get; set; }
        public void FishPickUp(ColoredFish fish);
        public void Restart();
    }
}