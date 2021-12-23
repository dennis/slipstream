#nullable enable

using Slipstream;
using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerEndpointRequested : IEvent
    {
        public string EventType => typeof(WebServerEndpointRequested).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Server { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string QueryParams { get; set; } = string.Empty;
    }
}