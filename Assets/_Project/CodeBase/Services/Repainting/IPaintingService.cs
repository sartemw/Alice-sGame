using System;
using System.Collections.Generic;
using _Project.CodeBase.Fish;
using UnityEngine;

namespace _Project.CodeBase.Services.Repainting
{
    public interface IPaintingService: IService
    {
        public event Action LevelOver;
        public List<Paintable> ColorlessObjs { get; }
        public List<Paintable> ColoredObjs{ get; }
        public Material Colorless{get;}
        public Material Colored {get;}

        public void Painting(ColoredFish fish);
        public void StartLevel();
        public void SetColorless(Paintable paintable);
    }
}