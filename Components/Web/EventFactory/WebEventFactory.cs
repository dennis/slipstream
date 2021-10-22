using Slipstream.Components.Web.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Web.EventFactory
{
    public class WebEventFactory : IWebEventFactory
    {
        public WebCommandData CreateWebCommandData(IEventEnvelope envelope, string route, string data)
        {
            return new WebCommandData { Envelope = envelope, Route = route, Data = data };
        }

        public WebCommandData CreateWebCommandData(IEventEnvelope envelope, string route, string clientId, string data)
        {
            return new WebCommandData { Envelope = envelope, Route = route, ClientId = clientId, Data = data };
        }

        public WebServerAdded CreateWebServerAdded(IEventEnvelope envelope, string endpoint)
        {
            return new WebServerAdded { Envelope = envelope, Endpoint = endpoint };
        }

        public WebServerRemoved CreateWebServerRemoved(IEventEnvelope envelope, string endpoint)
        {
            return new WebServerRemoved { Envelope = envelope, Endpoint = endpoint };
        }

        public WebSocketDataReceived CreateWebSocketDataReceived(IEventEnvelope envelope, string endpoint, string clientId, string data)
        {
            return new WebSocketDataReceived { Envelope = envelope, Data = data, Endpoint = endpoint, ClientId = clientId };
        }

        public WebSocketClientConnected CreateWebSocketClientConnected(IEventEnvelope envelope, string endpoint, string clientId)
        {
            return new WebSocketClientConnected { Envelope = envelope, Endpoint = endpoint, ClientId = clientId };
        }

        public WebSocketClientDisconnected CreateWebSocketClientDisconnected(IEventEnvelope envelope, string endpoint, string clientId)
        {
            return new WebSocketClientDisconnected { Envelope = envelope, Endpoint = endpoint, ClientId = clientId };
        }

        public WebCommandRouteStaticContent CreateWebCommandRouteStaticContent(IEventEnvelope envelope, string route, string mimeType, string content)
        {
            return new WebCommandRouteStaticContent { Envelope = envelope, Route = route, MimeType = mimeType, Content = content };
        }

        public WebCommandRoutePath CreateWebCommandRoutePath(IEventEnvelope envelope, string route, string path)
        {
            return new WebCommandRoutePath { Envelope = envelope, Route = route, Path = path };
        }

        public WebCommandRouteWebSocket CreateWebCommandRouteWebSocket(IEventEnvelope envelope, string route)
        {
            return new WebCommandRouteWebSocket { Envelope = envelope, Route = route };
        }

        public WebEndpointAdded CreateWebEndpointAdded(IEventEnvelope envelope, string endpoint, string url)
        {
            return new WebEndpointAdded { Envelope = envelope, Endpoint = endpoint, Url = url };
        }

        public WebEndpointRemoved CreateWebEndpointRemoved(IEventEnvelope envelope, string endpoint, string url)
        {
            return new WebEndpointRemoved { Envelope = envelope, Endpoint = endpoint, Url = url };
        }

        public WebEndpointRequested CreateWebEndpointRequested(IEventEnvelope envelope, string endpoint, string method, string body, string queryParams)
        {
            return new WebEndpointRequested { Envelope = envelope, Endpoint = endpoint, Method = method, Body = body, QueryParams = queryParams };
        }

        public WebCommandRouteFileContent CreateWebCommandRouteFileContent(IEventEnvelope envelope, string route, string mimeType, string filename)
        {
            return new WebCommandRouteFileContent
            {
                Envelope = envelope,
                Route = route,
                MimeType = mimeType,
                Filename = filename
            };
        }
    }
}