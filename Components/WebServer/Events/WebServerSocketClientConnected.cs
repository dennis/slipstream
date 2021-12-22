#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerSocketClientConnected : IEvent
    {
        public string EventType => typeof(WebServerSocketClientConnected).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
    }
}