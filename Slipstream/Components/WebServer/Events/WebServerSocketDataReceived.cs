#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerSocketDataReceived : IEvent
    {
        public string EventType => typeof(WebServerSocketDataReceived).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Server { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
    }
}