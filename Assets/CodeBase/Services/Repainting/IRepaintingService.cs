using System;
using System.Collections.Generic;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.Repainting
{
    public interface IRepaintingService: IService
    {
        event Action PickUpFish;
        public List<RepaintingData> RepaintingDatas { get;}
        public List<Repaintable> ColorlessObjs { get; }
        public List<Repaintable> ColoredObjs{ get; }
        public Material Colorless{get;}
        public  Material Colored {get;}
        public void Init(Material colorless, Material colored, ScalerPaintingMask.Factory maskFactory);
        public void Restart();
        
        public void AddFish(ColoredFish fish);
    }
}