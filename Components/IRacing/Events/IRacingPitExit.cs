#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPitExit : IEvent
    {
        public string EventType => nameof(IRacingPitExit);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public double? Duration { get; set; }
        public float? FuelLeft { get; set; }
    }
}