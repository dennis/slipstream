#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using Serilog;
using Slipstream.Components.WebWidget.EventHandler;
using Slipstream.Shared;

namespace Slipstream.Components.WebWidget
{
    public class HttpServer : IHttpServer, IHttpServerApi
    {
        private const string WEB_WIDGET_ROOT_DIRECTORY = "WebWidgets/";

        private readonly Object Lock = new object();
        private Thread? ServiceThread;
        private readonly ILogger Logger;
        private readonly IEventBusSubscription Subscription;
        private readonly WebSocketsEventsServer EventsServerModule;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IWebWidgetInstances Instances = new WebWidgetInstances();
        private const string Url = "http://127.0.0.1:1919"; // Must NOT end with slash

        public HttpServer(ILogger logger, IEventBusSubscription subscription, IEventHandlerController eventHandlerController)
        {
            Logger = logger;
            Subscription = subscription;
            EventHandlerController = eventHandlerController;

            EventsServerModule = new WebSocketsEventsServer(Logger, Instances);

            System.IO.Directory.CreateDirectory(WEB_WIDGET_ROOT_DIRECTORY);
        }

        public void AddInstance(string instanceId, string webWidgetType, string? data)
        {
            lock (Lock)
            {
                if (ServiceThread == null)
                {
                    ServiceThread = new Thread(new ThreadStart(ThreadMain))
                    {
                        Name = GetType().Name,
                    };
                    ServiceThread.Start();
                }
            }

            // Crude sanity check
            var indexFile = WEB_WIDGET_ROOT_DIRECTORY + webWidgetType + "/index.html";
            if (System.IO.File.Exists(indexFile))
            {
                Instances.Add(instanceId, webWidgetType, data);
                Subscription.AddImpersonate(instanceId);

                Logger.Information($"HttpServer: {Url}/instances/{instanceId} added");
            }
            else
            {
                Logger.Error($"HttpServer: {Url}/instances/{instanceId} not added, as {indexFile} does not exist");
            }
        }

        private void ThreadMain()
        {
            Logger.Information("HttpServer started");

            using (var server = CreateWebServer(Url))
            {
                Task serverTask = server.RunAsync();
                bool stopping = false;

                var internalHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
                var webWidgetHandler = EventHandlerController.Get<WebWidgetEventHandler>();

                internalHandler.OnInternalCommandShutdown += (_, e) => stopping = true;
                webWidgetHandler.OnWebWidgetCommandEvent += (_, e) =>
                {
                    if (e.Envelope.Recipients == null)
                        return;

                    foreach (var recipient in e.Envelope.Recipients)
                    {
                        EventsServerModule.Broadcast(recipient, e.Data);
                    }
                };

                while (!stopping)
                {
                    IEvent? @event = Subscription.NextEvent(100);
                    EventHandlerController.HandleEvent(@event);

                    stopping = stopping || serverTask.IsCompleted;
                }
            }

            Logger.Information("HttpServer stopped");
        }

        public void RemoveInstance(string instanceId)
        {
            Instances.Remove(instanceId);
            Subscription.DeleteImpersonation(instanceId);
            Logger.Information($"HttpServer: {Url}/instances/{instanceId} removed");
        }

        private WebServer CreateWebServer(string url)
        {
            // endpoints:
            // /
            // /ss.js
            // /instances/<instanceid>/
            // /webwidgets/<webwidgettype>/
            // /events/<instanceid>/

            var server = new WebServer(o => o
                .WithUrlPrefix(url))
                .WithStaticFolder("/webwidgets/", WEB_WIDGET_ROOT_DIRECTORY, false)
                .WithModule(EventsServerModule)
                .WithModule(new JavascriptWebModule())
                .WithModule(new InstanceWebModule(Instances, WEB_WIDGET_ROOT_DIRECTORY, Logger))
                .WithModule(new InstanceIndexWebModule(Instances, Logger));

            return server;
        }

        public void Dispose()
        {
        }
    }
}