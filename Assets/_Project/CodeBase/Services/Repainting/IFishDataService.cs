using System;
using _Project.CodeBase.Fish;

namespace _Project.CodeBase.Services.Repainting
{
    public interface IFishDataService: IService
    {
        public int FishOnLevel { get; set; }
        public void Restart();
    }
}