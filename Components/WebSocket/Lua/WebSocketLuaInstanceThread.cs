#nullable enable

using Newtonsoft.Json.Linq;

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using Swan.Parsers;

using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slipstream.Components.WebSocket.Lua
{
    public class WebSocketLuaInstanceThread : BaseInstanceThread, IWebSocketLuaInstanceThread
    {
        private const int MAX_READ_BUFFER_SIZE = 8192;

        private readonly IEventHandlerController EventHandlerController;
        private readonly string Endpoint;
        private readonly IEventFactory EventFactory;
        private readonly IEventBusSubscription Subscription;

        public WebSocketLuaInstanceThread(
            string luaLibraryName,
            string instanceId,
            string endpoint,
            Serilog.ILogger logger,
            IEventHandlerController eventHandlerController,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IEventBusSubscription eventBusSubscription,
            IInternalEventFactory internalEventFactory
        ) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            EventHandlerController = eventHandlerController;
            Endpoint = endpoint;
            EventFactory = eventFactory;
            EventBus = eventBus;
            Subscription = eventBusSubscription;
        }

        protected override void Main()
        {
            var cts = new CancellationTokenSource();
            var ws = new ClientWebSocket();

            var eventHandler = EventHandlerController.Get<WebSocketEventHandler>();

            eventHandler.OnWebSocketCommandData += (_, e) =>
            {
                var encoded = Encoding.UTF8.GetBytes(e.Data);
                var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
                ws.SendAsync(buffer, WebSocketMessageType.Text, true, cts.Token).GetAwaiter().GetResult();
            };

            ws.ConnectAsync(new Uri(Endpoint), cts.Token).GetAwaiter().GetResult();

            EventBus.PublishEvent(EventFactory.CreateWebSocketConnected(InstanceEnvelope));

            var buffer = new ArraySegment<byte>(new Byte[MAX_READ_BUFFER_SIZE]);
            var ms = new MemoryStream();

            var readTask = ws.ReceiveAsync(buffer, cts.Token);

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }

                if (ws.State != WebSocketState.Open)
                {
                    break;
                }

                if (readTask.IsCompleted)
                {
                    var result = readTask.GetAwaiter().GetResult();
                    ms.Write(buffer.Array!, buffer.Offset, result.Count);

                    if (result.EndOfMessage)
                    {
                        ms.Seek(0, SeekOrigin.Begin);

                        using var reader = new StreamReader(ms, Encoding.UTF8);
                        var data = reader.ReadToEnd();
                        ms.Dispose();

                        ms = new MemoryStream();

                        System.Diagnostics.Debug.WriteLine("GOT DATA: " + data);

                        EventBus.PublishEvent(EventFactory.CreateWebSocketDataReceived(InstanceEnvelope, data));
                    }

                    readTask = ws.ReceiveAsync(buffer, cts.Token);
                }
            }

            cts.Cancel();

            EventBus.PublishEvent(EventFactory.CreateWebSocketDisconnected(InstanceEnvelope));
        }

        public new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}