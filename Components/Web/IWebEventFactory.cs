using Slipstream.Components.Web.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Web
{
    public interface IWebEventFactory
    {
        WebCommandData CreateWebCommandData(IEventEnvelope envelope, string route, string data);

        WebCommandData CreateWebCommandData(IEventEnvelope envelope, string route, string clientId, string data);

        WebSocketDataReceived CreateWebSocketDataReceived(IEventEnvelope envelope, string endpoint, string clientId, string data);

        WebSocketClientConnected CreateWebSocketClientConnected(IEventEnvelope envelope, string endpoint, string clientId);

        WebSocketClientDisconnected CreateWebSocketClientDisconnected(IEventEnvelope envelope, string endpoint, string clientId);

        WebServerAdded CreateWebServerAdded(IEventEnvelope envelope, string endpoint);

        WebServerRemoved CreateWebServerRemoved(IEventEnvelope envelope, string endpoint);

        WebCommandRouteStaticContent CreateWebCommandRouteStaticContent(IEventEnvelope envelope, string route, string mimeType, string content);

        WebCommandRoutePath CreateWebCommandRoutePath(IEventEnvelope envelope, string route, string path);

        WebEndpointAdded CreateWebEndpointAdded(IEventEnvelope envelope, string endpoint, string url);

        WebEndpointRemoved CreateWebEndpointRemoved(IEventEnvelope envelope, string endpoint, string url);

        WebEndpointRequested CreateWebEndpointRequested(IEventEnvelope envelope, string endpoint, string method, string body, string queryParams);

        WebCommandRouteWebSocket CreateWebCommandRouteWebSocket(IEventEnvelope envelope, string route);

        WebCommandRouteFileContent CreateWebCommandRouteFileContent(IEventEnvelope envelope, string route, string mimeType, string filename);
    }
}