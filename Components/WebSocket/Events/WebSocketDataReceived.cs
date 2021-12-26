using Slipstream.Shared;

namespace Slipstream.Components.WebSocket.Events
{
    public class WebSocketDataReceived : IEvent
    {
        public string EventType => nameof(WebSocketDataReceived);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Data { get; set; } = string.Empty;
    }
}