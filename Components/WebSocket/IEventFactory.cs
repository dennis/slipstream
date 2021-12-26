#nullable enable

using Slipstream.Components.WebSocket.Events;
using Slipstream.Shared;

namespace Slipstream.Components.WebSocket
{
    public interface IEventFactory
    {
        WebSocketConnected CreateWebSocketConnected(IEventEnvelope envelope);

        WebSocketDisconnected CreateWebSocketDisconnected(IEventEnvelope envelope);

        WebSocketDataReceived CreateWebSocketDataReceived(IEventEnvelope envelope, string data);

        WebSocketCommandData CreateWebSocketCommandData(IEventEnvelope envelope, string data);
    }
}