using _Project.CodeBase.Services.Repainting;

namespace _Project.CodeBase.Events
{
    public class ShowCurtainSignal : IEventSignal {}
    public class ClipFinishSignal : IEventSignal {}
    public class BootstrapFinishedSignal : IEventSignal {}

    public class StartPaintingSignal : IEventSignal
    {
        public Paintable Target;
    }
}