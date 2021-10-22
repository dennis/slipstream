#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebEndpointRemoved : IEvent
    {
        public string EventType => typeof(WebEndpointRemoved).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}