﻿using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitChangeRightFrontTyre : IEvent
    {
        public string EventType => nameof(IRacingCommandPitChangeRightFrontTyre);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int Kpa { get; set; }
    }
}