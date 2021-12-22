#nullable enable

using Slipstream;
using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerEndpointRemoved : IEvent
    {
        public string EventType => typeof(WebServerEndpointRemoved).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}