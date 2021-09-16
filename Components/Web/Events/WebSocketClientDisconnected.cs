#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebSocketClientDisconnected : IEvent
    {
        public string EventType => typeof(WebSocketClientDisconnected).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
    }
}