using Slipstream.Shared;

namespace Slipstream.Components.WebSocket.Events
{
    public class WebSocketDisconnected : IEvent
    {
        public string EventType => nameof(WebSocketDisconnected);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
    }
}