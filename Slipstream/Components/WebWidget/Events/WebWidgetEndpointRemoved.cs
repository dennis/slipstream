#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebWidget.Events
{
    public class WebWidgetEndpointRemoved : IEvent
    {
        public string EventType => typeof(WebWidgetEndpointRemoved).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
    }
}