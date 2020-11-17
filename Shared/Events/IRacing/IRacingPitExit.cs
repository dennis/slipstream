#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingPitExit : IEvent
    {
        public string EventType => "IRacingPitExit";
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public double? Duration { get; set; }
    }
}
