﻿using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitChangeLeftRearTyre : IEvent
    {
        public string EventType => nameof(IRacingCommandPitChangeLeftRearTyre);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int Kpa { get; set; }
    }
}