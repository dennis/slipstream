using Slipstream.Shared;

namespace Slipstream.Components.WebSocket.Events
{
    public class WebSocketCommandData : IEvent
    {
        public string EventType => nameof(WebSocketCommandData);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Data { get; set; } = string.Empty;
    }
}