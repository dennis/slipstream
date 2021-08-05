#nullable enable

using System;

using Serilog;

namespace Slipstream.Components.WebWidget.Lua
{
    public class WebWidgetInstanceThread : IWebWidgetInstanceThread
    {
        private readonly string InstanceId;
        private readonly string WebWidgetType;
        private readonly IHttpServerApi HttpServer;
        private readonly string? Data;

        public WebWidgetInstanceThread(string instanceId, string webWidgetType, string data, IHttpServerApi httpServer)
        {
            InstanceId = instanceId;
            WebWidgetType = webWidgetType;
            HttpServer = httpServer;
            Data = data;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            HttpServer.AddInstance(InstanceId, WebWidgetType, Data);
        }

        public void Stop()
        {
            HttpServer.RemoveInstance(InstanceId);
        }
    }
}