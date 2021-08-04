#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebWidget.Events
{
    public class WebWidgetCommandEvent : IEvent
    {
        public string EventType => typeof(WebWidgetCommandEvent).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Data { get; set; } = string.Empty;
    }
}