namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingSessionState : IEvent
    {
        public enum StateEnum
        {
            GetInCar,
            Warmup,
            ParadeLaps,
            Racing,
            Checkered,
            CoolDown,
            Invalid
        }

        public string EventType => "IRacingSessionState";
        public double SessionTime { get; set; }
        public StateEnum State { get; set; } = StateEnum.Invalid;

        public bool DifferentTo(IRacingSessionState other)
        {
            return State.Equals(other.State);
        }
    }
}
