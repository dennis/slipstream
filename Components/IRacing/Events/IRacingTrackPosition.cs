#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTrackPosition : IEvent
    {
        public string EventType => nameof(IRacingTrackPosition);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public int CurrentPositionInClass { get; set; }
        public int CurrentPositionInRace { get; set; }
        public int PreviousPositionInClass { get; set; }
        public int PreviousPositionInRace { get; set; }
        public int[] NewCarsAhead { get; set; } = new int[] { };
        public int[] NewCarsBehind { get; set; } = new int[] { };
    }
}