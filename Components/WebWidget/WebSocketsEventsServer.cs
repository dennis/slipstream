#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using EmbedIO.WebSockets;
using Serilog;

namespace Slipstream.Components.WebWidget
{
    public class WebSocketsEventsServer : WebSocketModule
    {
        private readonly ILogger Logger;
        private readonly IWebWidgetInstances Instances;
        private readonly Dictionary<string, List<string>> InstanceToContextIdMap = new Dictionary<string, List<string>>();

        public WebSocketsEventsServer(ILogger logger, IWebWidgetInstances instances) : base("/events/{id}", true)
        {
            Logger = logger;
            Instances = instances;
        }

        protected override Task OnMessageReceivedAsync(IWebSocketContext context, byte[] rxBuffer, IWebSocketReceiveResult rxResult)
            => Task.CompletedTask;

        protected override Task OnClientConnectedAsync(IWebSocketContext context)
        {
            string instanceId = ParseInstanceId(context);
            Logger.Information($"HttpServer - connected {context} - ctxid={context.Id}, instanceID={instanceId}");

            lock (InstanceToContextIdMap)
            {
                if (!InstanceToContextIdMap.ContainsKey(instanceId))
                    InstanceToContextIdMap.Add(instanceId, new List<string>());

                InstanceToContextIdMap[instanceId].Add(context.Id);
            }

            var initData = Instances.InitData(instanceId);

            // if we encoded null, it will be "null" - no need to send that
            if (initData != null && initData != "null")
                Broadcast(instanceId, initData);

            return base.OnClientConnectedAsync(context);
        }

        private static string ParseInstanceId(IWebSocketContext context)
        {
            return context.RequestUri.LocalPath.Substring("/events/".Length);
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
                List<string> ctxIds;
                InstanceToContextIdMap.TryGetValue(instanceId, out ctxIds);

                return ctxIds != null && ctxIds.Contains(s.Id);
            });
        }
    }
}