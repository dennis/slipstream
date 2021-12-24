#nullable enable

using Slipstream.Components.WebServer.Events;
using Slipstream.Shared;

using System;

namespace Slipstream.Components.WebServer.EventHandler
{
    internal class WebServerEventHandler : IEventHandler
    {
        public event EventHandler<WebServerCommandData>? OnWebServerCommandData;

        public event EventHandler<WebServerServerAdded>? OnWebServerServerAdded;

        public event EventHandler<WebServerServerRemoved>? OnWebServerRemoved;

        public event EventHandler<WebServerSocketDataReceived>? OnWebServerSocketDataReceived;

        public event EventHandler<WebServerSocketClientConnected>? OnWebServerSocketClientConnected;

        public event EventHandler<WebServerSocketClientDisconnected>? OnWebServerSocketClientDisconnected;

        public event EventHandler<WebServerCommandRouteStaticContent>? OnWebServerCommandRouteStaticContent;

        public event EventHandler<WebServerCommandRoutePath>? OnWebServerCommandRoutePath;

        public event EventHandler<WebServerEndpointAdded>? OnWebServerEndpointAdded;

        public event EventHandler<WebServerEndpointRemoved>? OnWebServerEndpointRemoved;

        public event EventHandler<WebServerEndpointRequested>? OnWebServerEndpointRequested;

        public event EventHandler<WebServerCommandRouteWebSocket>? OnWebServerCommandRouteWebSocket;

        public event EventHandler<WebServerCommandRouteFileContent>? OnWebServerCommandRouteFileContent;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                WebServerCommandData tev => OnEvent(OnWebServerCommandData, tev),
                WebServerServerAdded tev => OnEvent(OnWebServerServerAdded, tev),
                WebServerServerRemoved tev => OnEvent(OnWebServerRemoved, tev),
                WebServerEndpointAdded tev => OnEvent(OnWebServerEndpointAdded, tev),
                WebServerEndpointRemoved tev => OnEvent(OnWebServerEndpointRemoved, tev),
                WebServerEndpointRequested tev => OnEvent(OnWebServerEndpointRequested, tev),
                WebServerCommandRouteWebSocket tev => OnEvent(OnWebServerCommandRouteWebSocket, tev),
                WebServerSocketDataReceived tev => OnEvent(OnWebServerSocketDataReceived, tev),
                WebServerSocketClientConnected tev => OnEvent(OnWebServerSocketClientConnected, tev),
                WebServerSocketClientDisconnected tev => OnEvent(OnWebServerSocketClientDisconnected, tev),
                WebServerCommandRouteStaticContent tev => OnEvent(OnWebServerCommandRouteStaticContent, tev),
                WebServerCommandRoutePath tev => OnEvent(OnWebServerCommandRoutePath, tev),
                WebServerCommandRouteFileContent tev => OnEvent(OnWebServerCommandRouteFileContent, tev),
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