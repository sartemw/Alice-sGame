using UnityEngine;

namespace Assets.EventBus.Example.Events
{
    public class GreenClickSignal : IEventSignal { }
    public class RedClickSignal : IEventSignal { }
    public class MoveClickSignal : IEventSignal
    {
        public Vector3 direction;
    }
}