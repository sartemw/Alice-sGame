using System;
using _Project.CodeBase.Fish;
using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [Serializable]
    public class FishSpawnerStaticData
    {
        public string Id;
        public ColorType FishColor;
        public FishBehaviourEnum FishBehaviour;
        public Vector2 Position;

        public FishSpawnerStaticData(string id, ColorType colorType, FishBehaviourEnum fishBehaviour, Vector2 position)
        {
            Id = id;
            FishColor = colorType;
            FishBehaviour = fishBehaviour;
            Position = position;
        }
    }
}