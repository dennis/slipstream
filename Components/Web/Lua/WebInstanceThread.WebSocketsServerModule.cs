#nullable enable

using System.Text;
using System.Threading.Tasks;

using EmbedIO.WebSockets;

using Serilog;

using Slipstream.Shared;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class WebSocketsServerModule : WebSocketModule
        {
            private readonly IEventBus EventBus;
            private readonly IWebEventFactory WebEventFactory;
            private readonly IEventEnvelope Envelope;
            private readonly ILogger Logger;
            private readonly string Endpoint;

            public WebSocketsServerModule(
                IEventBus eventBus,
                IWebEventFactory webEventFactory,
                IEventEnvelope envelope,
                ILogger logger,
                string route
                ) : base(route, true)
            {
                EventBus = eventBus;
                WebEventFactory = webEventFactory;
                Envelope = envelope;
                Logger = logger;
                Endpoint = route;

                Logger.Information($"Web: WS: Ready for connections on {route}");
            }

            protected override Task OnClientConnectedAsync(IWebSocketContext context)
            {
                Logger.Information($"Web: WS Client Connected {context.Id}");

                EventBus.PublishEvent(WebEventFactory.CreateWebSocketClientConnected(Envelope, Endpoint, context.Id));

                return base.OnClientConnectedAsync(context);
            }

            protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
            {
                Logger.Information($"Web: WS Client Disconnected {context.Id}");

                EventBus.PublishEvent(WebEventFactory.CreateWebSocketClientDisconnected(Envelope, Endpoint, context.Id));

                return base.OnClientDisconnectedAsync(context);
            }

            protected override Task OnMessageReceivedAsync(IWebSocketContext context, byte[] rxBuffer, IWebSocketReceiveResult rxResult)
            {
                var data = Encoding.UTF8.GetString(rxBuffer);
                Logger.Information($"HttpServer - message from {context.Id}: {data}");

                EventBus.PublishEvent(WebEventFactory.CreateWebSocketDataReceived(Envelope, Endpoint, context.Id, data));

                return Task.CompletedTask;
            }

            public void SendMessage(string clientId, string data)
            {
                BroadcastAsync(data, s => s.Id == clientId || clientId == "");
            }
        }
    }
}