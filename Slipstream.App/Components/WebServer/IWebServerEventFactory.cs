using Slipstream.Components.WebServer.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WebServer
{
    public interface IWebServerEventFactory
    {
        WebServerCommandData CreateWebServerCommandData(IEventEnvelope envelope, string route, string data);

        WebServerCommandData CreateWebServerCommandData(IEventEnvelope envelope, string route, string clientId, string data);

        WebServerSocketDataReceived CreateWebServerSocketDataReceived(IEventEnvelope envelope, string server, string endpoint, string clientId, string data);

        WebServerSocketClientConnected CreateWebServerSocketClientConnected(IEventEnvelope envelope, string server, string endpoint, string clientId);

        WebServerSocketClientDisconnected CreateWebServerSocketClientDisconnected(IEventEnvelope envelope, string server, string endpoint, string clientId);

        WebServerServerAdded CreateWebServerServerAdded(IEventEnvelope envelope, string server, string endpoint);

        WebServerServerRemoved CreateWebServerServerRemoved(IEventEnvelope envelope, string server, string endpoint);

        WebServerCommandRouteStaticContent CreateWebServerCommandRouteStaticContent(IEventEnvelope envelope, string route, string mimeType, string content);

        WebServerCommandRoutePath CreateWebServerCommandRoutePath(IEventEnvelope envelope, string route, string path);

        WebServerEndpointAdded CreateWebServerEndpointAdded(IEventEnvelope envelope, string server, string endpoint, string url);

        WebServerEndpointRemoved CreateWebServerEndpointRemoved(IEventEnvelope envelope, string server, string endpoint, string url);

        WebServerEndpointRequested CreateWebServerEndpointRequested(IEventEnvelope envelope, string server, string endpoint, string method, string body, string queryParams);

        WebServerCommandRouteWebSocket CreateWebServerCommandRouteWebSocket(IEventEnvelope envelope, string route);

        WebServerCommandRouteFileContent CreateWebServerCommandRouteFileContent(IEventEnvelope envelope, string route, string mimeType, string filename);
    }
}