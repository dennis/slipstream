﻿using Slipstream.Components.WebServer.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WebServer.EventFactory
{
    public class WebServerEventFactory : IWebServerEventFactory
    {
        public WebServerCommandData CreateWebServerCommandData(IEventEnvelope envelope, string route, string data)
        {
            return new WebServerCommandData { Envelope = envelope, Route = route, Data = data };
        }

        public WebServerCommandData CreateWebServerCommandData(IEventEnvelope envelope, string route, string clientId, string data)
        {
            return new WebServerCommandData { Envelope = envelope, Route = route, ClientId = clientId, Data = data };
        }

        public WebServerServerAdded CreateWebServerServerAdded(IEventEnvelope envelope, string endpoint)
        {
            return new WebServerServerAdded { Envelope = envelope, Endpoint = endpoint };
        }

        public WebServerServerRemoved CreateWebServerServerRemoved(IEventEnvelope envelope, string endpoint)
        {
            return new WebServerServerRemoved { Envelope = envelope, Endpoint = endpoint };
        }

        public WebServerSocketDataReceived CreateWebServerSocketDataReceived(IEventEnvelope envelope, string endpoint, string clientId, string data)
        {
            return new WebServerSocketDataReceived { Envelope = envelope, Data = data, Endpoint = endpoint, ClientId = clientId };
        }

        public WebServerSocketClientConnected CreateWebServerSocketClientConnected(IEventEnvelope envelope, string endpoint, string clientId)
        {
            return new WebServerSocketClientConnected { Envelope = envelope, Endpoint = endpoint, ClientId = clientId };
        }

        public WebServerSocketClientDisconnected CreateWebServerSocketClientDisconnected(IEventEnvelope envelope, string endpoint, string clientId)
        {
            return new WebServerSocketClientDisconnected { Envelope = envelope, Endpoint = endpoint, ClientId = clientId };
        }

        public WebServerCommandRouteStaticContent CreateWebServerCommandRouteStaticContent(IEventEnvelope envelope, string route, string mimeType, string content)
        {
            return new WebServerCommandRouteStaticContent { Envelope = envelope, Route = route, MimeType = mimeType, Content = content };
        }

        public WebServerCommandRoutePath CreateWebServerCommandRoutePath(IEventEnvelope envelope, string route, string path)
        {
            return new WebServerCommandRoutePath { Envelope = envelope, Route = route, Path = path };
        }

        public WebServerCommandRouteWebSocket CreateWebServerCommandRouteWebSocket(IEventEnvelope envelope, string route)
        {
            return new WebServerCommandRouteWebSocket { Envelope = envelope, Route = route };
        }

        public WebServerEndpointAdded CreateWebServerEndpointAdded(IEventEnvelope envelope, string endpoint, string url)
        {
            return new WebServerEndpointAdded { Envelope = envelope, Endpoint = endpoint, Url = url };
        }

        public WebServerEndpointRemoved CreateWebServerEndpointRemoved(IEventEnvelope envelope, string endpoint, string url)
        {
            return new WebServerEndpointRemoved { Envelope = envelope, Endpoint = endpoint, Url = url };
        }

        public WebServerEndpointRequested CreateWebServerEndpointRequested(IEventEnvelope envelope, string endpoint, string method, string body, string queryParams)
        {
            return new WebServerEndpointRequested { Envelope = envelope, Endpoint = endpoint, Method = method, Body = body, QueryParams = queryParams };
        }

        public WebServerCommandRouteFileContent CreateWebServerCommandRouteFileContent(IEventEnvelope envelope, string route, string mimeType, string filename)
        {
            return new WebServerCommandRouteFileContent
            {
                Envelope = envelope,
                Route = route,
                MimeType = mimeType,
                Filename = filename
            };
        }
    }
}