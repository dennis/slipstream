#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using EmbedIO.WebSockets;

using Serilog;
using Slipstream.Shared;

namespace Slipstream.Components.WebWidget
{
    public class WebSocketsEventsServer : WebSocketModule
    {
        private readonly ILogger Logger;
        private readonly IWebWidgetInstances Instances;
        private readonly IEventBus EventBus;
        private readonly IWebWidgetEventFactory WebWidgetEventFactory;
        private readonly Dictionary<string, List<string>> InstanceToContextIdMap = new Dictionary<string, List<string>>();

        public WebSocketsEventsServer(
            ILogger logger,
            IWebWidgetInstances instances,
            IEventBus eventBus,
            IWebWidgetEventFactory webWidgetEventFactory
        ) : base("/events/{id}", true)
        {
            Logger = logger;
            Instances = instances;
            EventBus = eventBus;
            WebWidgetEventFactory = webWidgetEventFactory;
        }

        protected override Task OnMessageReceivedAsync(IWebSocketContext context, byte[] rxBuffer, IWebSocketReceiveResult rxResult)
        {
            string instanceId = ParseInstanceId(context);
            var data = Encoding.UTF8.GetString(rxBuffer);
            Logger.Information($"HttpServer - got data from instanceID={instanceId}: {data}");

            EventBus.PublishEvent(WebWidgetEventFactory.CreateWebWidgetData(new EventEnvelope(instanceId), data));

            return Task.CompletedTask;
        }

        protected override Task OnClientConnectedAsync(IWebSocketContext context)
        {
            string instanceId = ParseInstanceId(context);
            Logger.Information($"HttpServer - connected to instanceID={instanceId}");

            lock (InstanceToContextIdMap)
            {
                if (!InstanceToContextIdMap.ContainsKey(instanceId))
                    InstanceToContextIdMap.Add(instanceId, new List<string>());

                InstanceToContextIdMap[instanceId].Add(context.Id);
            }

            var initData = Instances[instanceId].InitData;

            // if we encoded null, it will be "null" - no need to send that
            if (initData != null && initData != "null")
                Broadcast(instanceId, initData);

            return base.OnClientConnectedAsync(context);
        }

        private static string ParseInstanceId(IWebSocketContext context)
        {
            return context.RequestUri.LocalPath["/events/".Length..];
        }

        protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
        {
            string instanceId = ParseInstanceId(context);
            Logger.Information($"HttpServer - disconnected {context.RequestUri.LocalPath} - ctxid={context.Id}, instanceId={instanceId}");

            lock (InstanceToContextIdMap)
            {
                if (InstanceToContextIdMap.ContainsKey(instanceId))
                    InstanceToContextIdMap[instanceId].Remove(context.Id);
            }

            return base.OnClientDisconnectedAsync(context);
        }

        public void Broadcast(string instanceId, string data)
        {
            BroadcastAsync(data, s =>
            {
                InstanceToContextIdMap.TryGetValue(instanceId, out List<string>? ctxIds);

                return ctxIds != null && ctxIds.Contains(s.Id);
            });
        }
    }
}