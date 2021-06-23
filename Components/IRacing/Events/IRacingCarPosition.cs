#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCarPosition : IEvent
    {
        public string EventType => nameof(IRacingCarPosition);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public int PositionInClass { get; set; }
        public int PositionInRace { get; set; }
    }
}