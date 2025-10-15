using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.CodeBase.Services.Repainting
{
    public interface IPaintingService: IService
    {
        public List<Paintable> ColorlessObjs { get; }
        public List<Paintable> ColoredObjs{ get; }
        public Material ColorlessMaterial{get;}
        public Material Colored {get;}

        public void StartLevel();
        public void SetColorless(Paintable paintable);
        public void CheckLevelCompletedOnStart();
        public void IsStart(bool value);
    }
}