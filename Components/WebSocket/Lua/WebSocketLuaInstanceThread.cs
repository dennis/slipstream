#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System;
using System.Collections.Concurrent;
using System.IO;
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
            Task.Run(() => MainAsync());
        }

        private async void MainAsync()
        {
            using var cts = new CancellationTokenSource();
            var outgoingData = new BlockingCollection<ArraySegment<byte>>();

            var eventHandler = EventHandlerController.Get<WebSocketEventHandler>();
            eventHandler.OnWebSocketCommandData += (_, e) =>
            {
                var encoded = Encoding.UTF8.GetBytes(e.Data);
                var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

                outgoingData.Add(buffer);
            };

            var eventHandlerTask = EventHandlerAsync(cts.Token);

            while (!Stopping)
            {
                var announceConnected = false;
                try
                {
                    // Some webserver requires Origin (e.g the one i use in go) - so let's make one
                    var ws = new ClientWebSocket();
                    ws.Options.SetRequestHeader("Origin", Endpoint.Replace("wss://", "https://").Replace("ws://", "http://"));

                    await ws.ConnectAsync(new Uri(Endpoint), cts.Token);
                    announceConnected = true;
                    Logger.Information("{InstanceId} is connected to {Endpoint}", InstanceId, Endpoint);
                    EventBus.PublishEvent(EventFactory.CreateWebSocketConnected(InstanceEnvelope));

                    var buffer = new ArraySegment<byte>(new Byte[MAX_READ_BUFFER_SIZE]);
                    var ms = new MemoryStream();

                    var readTask = ws.ReceiveAsync(buffer, cts.Token);
                    var pendingWriteTask = GetNextOutgoingData(outgoingData, cts.Token);

                    while (!Stopping)
                    {
                        if (ws.State != WebSocketState.Open)
                        {
                            break;
                        }

                        var completedTask = await Task.WhenAny(new Task[] { readTask, pendingWriteTask, eventHandlerTask });

                        if (completedTask == readTask)
                        {
                            var result = readTask.GetAwaiter().GetResult(); // we want it to throw exceptions, which it doesnt do with readTask.Result

                            ms.Write(buffer.Array!, buffer.Offset, result.Count);

                            if (result.Count == 0)
                            {
                                // Graceful disconnect
                                Logger.Information("{InstanceId} {Endpoint} disconnected", InstanceId, Endpoint);
                                break;
                            }

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
                        else if (completedTask == eventHandlerTask)
                        {
                            // Nothing to do - it will only complete once we're outside this while() loop
                        }
                        else if (completedTask == pendingWriteTask)
                        {
                            if (completedTask.IsCompletedSuccessfully)
                            {
                                await ws.SendAsync(pendingWriteTask.Result, WebSocketMessageType.Text, true, cts.Token);
                                pendingWriteTask = GetNextOutgoingData(outgoingData, cts.Token);
                            }
                        }
                        else
                        {
                            // This should never happen
                            Logger.Debug("{InstanceId} - this should never happen", InstanceId);
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    Logger.Error(e, "{InstanceId} cannot connect to {Endpoint}", InstanceId, Endpoint);
                    continue;
                }
                finally
                {
                    if (announceConnected)
                    {
                        EventBus.PublishEvent(EventFactory.CreateWebSocketDisconnected(InstanceEnvelope));
                        announceConnected = false;
                    }
                }
            }

            cts.Cancel();
        }

        private static Task<ArraySegment<byte>> GetNextOutgoingData(BlockingCollection<ArraySegment<byte>> outgoingData, CancellationToken token)
        {
            return Task.Run(() => outgoingData.Take(token), token);
        }

        private Task EventHandlerAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    IEvent? @event = Subscription.NextEvent(100);
                    if (@event != null)
                    {
                        EventHandlerController.HandleEvent(@event);
                    }
                }
            }, token);
        }

        public new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}