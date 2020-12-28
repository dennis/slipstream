#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingSessionState : IEvent
    {
        public string EventType => "IRacingSessionState";
        public double SessionTime { get; set; }
        public string State { get; set; } = ""; // Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup

        public bool DifferentTo(IRacingSessionState other)
        {
            return State.Equals(other.State);
        }
    }
}
