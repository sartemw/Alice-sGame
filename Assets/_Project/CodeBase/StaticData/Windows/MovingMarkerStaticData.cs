using System;
using _Project.CodeBase.Enemy.Base;
using UnityEngine;

namespace _Project.CodeBase.StaticData.Windows
{
    [Serializable]
    public class MovingMarkerStaticData
    {
        public string Id;
        public PointTypeId PointTypeId;
        public Vector3 Position;
        
        public MovingMarkerStaticData(string id, PointTypeId pointTypeId, Vector3 position)
        {
            Id = id;
            PointTypeId = pointTypeId;
            Position = position;
        }
    }
}