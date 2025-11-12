using UnityEngine;

namespace _Project.CodeBase.Events
{
    public class SandInkToBlotSignals : IEventSignal
    {
        public Vector2 At;
        public Transform To;
    }

    public class NextSceneSignal : IEventSignal
    {
        public string Scene;
    }
}