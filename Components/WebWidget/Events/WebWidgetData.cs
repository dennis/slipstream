﻿#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebWidget.Events
{
    public class WebWidgetData : IEvent
    {
        public string EventType => typeof(WebWidgetData).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Data { get; set; } = string.Empty;
    }
}