using System.Collections.Generic;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.FishCollectorService
{
    public interface IRepaintingService: IService
    {
        public List<GameObject> ColorlessObjs { get; }
        public List<GameObject> ColorledObjs{ get; }
        public void Init();
        
        public void AddFish(ColoredFish fish);
    }
}