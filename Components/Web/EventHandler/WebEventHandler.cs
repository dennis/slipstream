#nullable enable

using Slipstream.Components.Web.Events;
using Slipstream.Shared;

using System;

namespace Slipstream.Components.Web.EventHandler
{
    internal class WebEventHandler : IEventHandler
    {
        public event EventHandler<WebCommandData>? OnWebCommandData;

        public event EventHandler<WebServerAdded>? OnWebServerAdded;

        public event EventHandler<WebServerRemoved>? OnWebServerRemoved;

        public event EventHandler<WebSocketDataReceived>? OnWebSocketDataReceived;

        public event EventHandler<WebSocketClientConnected>? OnWebSocketClientConnected;

        public event EventHandler<WebSocketClientDisconnected>? OnWebSocketClientDisconnected;

        public event EventHandler<WebCommandRouteStaticContent>? OnWebCommandRouteStaticContent;

        public event EventHandler<WebCommandRoutePath>? OnWebCommandRoutePath;

        public event EventHandler<WebEndpointAdded>? OnWebEndpointAdded;

        public event EventHandler<WebEndpointRemoved>? OnWebEndpointRemoved;

        public event EventHandler<WebEndpointRequested>? OnWebEndpointRequested;

        public event EventHandler<WebCommandRouteWebSocket>? OnWebCommandRouteWebSocket;

        public event EventHandler<WebCommandRouteFileContent>? OnWebCommandRouteFileContent;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                WebCommandData tev => OnEvent(OnWebCommandData, tev),
                WebServerAdded tev => OnEvent(OnWebServerAdded, tev),
                WebServerRemoved tev => OnEvent(OnWebServerRemoved, tev),
                WebEndpointAdded tev => OnEvent(OnWebEndpointAdded, tev),
                WebEndpointRemoved tev => OnEvent(OnWebEndpointRemoved, tev),
                WebEndpointRequested tev => OnEvent(OnWebEndpointRequested, tev),
                WebCommandRouteWebSocket tev => OnEvent(OnWebCommandRouteWebSocket, tev),
                WebSocketDataReceived tev => OnEvent(OnWebSocketDataReceived, tev),
                WebSocketClientConnected tev => OnEvent(OnWebSocketClientConnected, tev),
                WebSocketClientDisconnected tev => OnEvent(OnWebSocketClientDisconnected, tev),
                WebCommandRouteStaticContent tev => OnEvent(OnWebCommandRouteStaticContent, tev),
                WebCommandRoutePath tev => OnEvent(OnWebCommandRoutePath, tev),
                WebCommandRouteFileContent tev => OnEvent(OnWebCommandRouteFileContent, tev),
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