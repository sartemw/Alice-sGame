using System;
using System.Collections.Generic;
using _Project.CodeBase.Enemy.Base;
using _Project.CodeBase.Infrastructure.States;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.StaticData.Windows;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.CodeBase.Enemy
{
    public class BossStats : MonoBehaviour
    {
        public IAudioService AudioService;
        public Transform HeroTransform;

        public MovingMarkerStaticData CurrentPoint;
        public MovingMarkerStaticData StartPoint;
        public MovingMarkerStaticData[] MovingPoints;
        public MovingMarkerStaticData EndPoint;
        public float Speed;

        public GameObject BlotInk;
        public IGameStateMachine StateMachine { get; set; }

        public void Construct(Transform heroTransform, float speed, List<MovingMarkerStaticData> points, IAudioService audioService, IGameStateMachine stateMachine)
        {
            Speed = speed;
            HeroTransform = heroTransform;
            SetupPoints(points);
            AudioService = audioService;
            StateMachine = stateMachine;
        }
        
        private void SetupPoints(List<MovingMarkerStaticData> points)
        {
            MovingPoints = new MovingMarkerStaticData[points.Count];
            
            foreach (MovingMarkerStaticData point in points)
            {
                switch (point.PointTypeId)
                {
                    case PointTypeId.StartPosition:
                        StartPoint = point;
                        MovingPoints[0] = point;
                        break;
                    case PointTypeId.EndPosition:
                        EndPoint = point;
                        MovingPoints[points.Count - 1] = point;
                        break;
                    case PointTypeId.Position_1:
                        MovingPoints[1] = point;
                        break;
                    case PointTypeId.Position_2:
                        MovingPoints[2] = point;
                        break;
                    case PointTypeId.Position_3:
                        MovingPoints[3] = point;
                        break;
                    case PointTypeId.Position_4:
                        MovingPoints[4] = point;
                        break;
                    case PointTypeId.Position_5:
                        MovingPoints[5] = point;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}