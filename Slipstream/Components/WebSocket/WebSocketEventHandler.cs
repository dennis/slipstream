#nullable enable

using Slipstream.Components.WebSocket.Events;
using Slipstream.Shared;

using System;

namespace Slipstream.Components
{
    internal class WebSocketEventHandler : IEventHandler
    {
        public event EventHandler<WebSocketConnected>? OnWebSocketConnected;

        public event EventHandler<WebSocketDisconnected>? OnWebSocketDisconnected;

        public event EventHandler<WebSocketDataReceived>? OnWebSocketDataReceived;

        public event EventHandler<WebSocketCommandData>? OnWebSocketCommandData;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                WebSocketConnected tev => OnEvent(OnWebSocketConnected, tev),
                WebSocketDisconnected tev => OnEvent(OnWebSocketDisconnected, tev),
                WebSocketDataReceived tev => OnEvent(OnWebSocketDataReceived, tev),
                WebSocketCommandData tev => OnEvent(OnWebSocketCommandData, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}