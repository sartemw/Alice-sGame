using System;
using System.Collections.Generic;
using CodeBase.Fish;
using CodeBase.Mask;
using UnityEngine;

namespace CodeBase.Services.Repainting
{
    public interface IRepaintingService: IService
    {
        public event Action LevelOver;
        public List<RepaintingData> RepaintingDatas { get;}
        public List<Repaintable> ColorlessObjs { get; }
        public List<Repaintable> ColoredObjs{ get; }
        public Material Colorless{get;}
        public  Material Colored {get;}

        public void Painting(GameObject paintingObject);
        public void RePainting(GameObject paintingObject);
        public void Restart();
    }
}