﻿#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.UI.Events
{
    public class UICommandWriteToConsole : IEvent
    {
        public string EventType => nameof(UICommandWriteToConsole);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string? Message { get; set; }
    }
}