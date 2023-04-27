using System;
using System.Collections.Generic;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.FishCollectorService
{
    public interface IRepaintingService: IService
    {
        event Action PickUpFish;
        public List<GameObject> ColorlessObjs { get; }
        public List<GameObject> ColorledObjs{ get; }
        public Material Colorless{get;}
        public  Material Colored {get;}
        public void Init(Material colorless, Material colored);
        public void Restart();
        
        public void AddFish(ColoredFish fish);
    }
}