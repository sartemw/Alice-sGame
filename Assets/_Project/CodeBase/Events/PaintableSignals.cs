using _Project.CodeBase.Fish;
using _Project.CodeBase.Services.Repainting;

namespace _Project.CodeBase.Events
{
    public class FishPickupSignal : IEventSignal
    {
        public ColoredFish ColoredFish;
    }

    public class StartPaintingSignal : IEventSignal
    {
        public Paintable Target;
    }
    public class StartPaintingInstantlySignal : IEventSignal
    {
        public Paintable Target;
    }

    public class StartFadeSignal : IEventSignal {}
    public class FadeMaterialSignal : IEventSignal 
    {
        public Paintable Target;
    }

    public class PaintingCompletedSignal : IEventSignal { }
    
    public class AllLevelColoringSignal : IEventSignal {}
}