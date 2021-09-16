#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebEndpointRequested : IEvent
    {
        public string EventType => typeof(WebEndpointRequested).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string QueryParams { get; set; } = string.Empty;
    }
}