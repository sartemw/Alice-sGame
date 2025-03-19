using _Project.CodeBase.Fish;

namespace _Project.CodeBase.EventBus.Events
{
    public class FishPickupSignal : IEventSignal
    {
        public ColoredFish ColoredFish;
    }
}