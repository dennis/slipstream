using Slipstream.Shared;

namespace Slipstream.Components.WebSocket.Events
{
    public class WebSocketConnected : IEvent
    {
        public string EventType => nameof(WebSocketConnected);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
    }
}