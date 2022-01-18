#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebWidget.Events
{
    public class WebWidgetEndpointAdded : IEvent
    {
        public string EventType => typeof(WebWidgetEndpointAdded).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
    }
}