﻿using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateLatestVersionChanged : IEvent
    {
        public string EventType => nameof(ApplicationUpdateLatestVersionChanged);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string LatestVersion { get; set; } = string.Empty;
    }
}