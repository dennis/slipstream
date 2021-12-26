using Slipstream.Components.WebSocket.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WebSocket.EventFactory
{
    public class EventFactory : IEventFactory
    {
        public WebSocketConnected CreateWebSocketConnected(IEventEnvelope envelope)
        {
            return new WebSocketConnected
            {
                Envelope = envelope,
            };
        }

        public WebSocketDisconnected CreateWebSocketDisconnected(IEventEnvelope envelope)
        {
            return new WebSocketDisconnected
            {
                Envelope = envelope,
            };
        }

        public WebSocketDataReceived CreateWebSocketDataReceived(IEventEnvelope envelope, string data)
        {
            return new WebSocketDataReceived()
            {
                Envelope = envelope,
                Data = data,
            };
        }

        public WebSocketCommandData CreateWebSocketCommandData(IEventEnvelope envelope, string data)
        {
            return new WebSocketCommandData()
            {
                Envelope = envelope,
                Data = data,
            };
        }
    }
}