#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerEndpointAdded : IEvent
    {
        public string EventType => typeof(WebServerEndpointAdded).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Server { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}