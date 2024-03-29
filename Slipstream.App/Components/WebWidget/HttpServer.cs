﻿#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

using EmbedIO;

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Components.WebWidget.EventHandler;
using Slipstream.Components.WebWidget.Lua;
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
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly WebWidgetLuaLibrary LuaLibrary;
        private readonly IWebWidgetEventFactory WebWidgetEventFactory;
        private readonly IWebWidgetInstances Instances = new WebWidgetInstances();
        private readonly IEventEnvelope BroadcastEnvelope = new EventEnvelope("webwidget");
        private const string Url = "http://*:1919"; // Must NOT end with slash
        private volatile bool Stopping;

        public HttpServer(
            ILogger logger,
            IEventBusSubscription subscription,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IInternalEventFactory internalEventFactory,
            IWebWidgetEventFactory webWidgetEventFactory,
            WebWidgetLuaLibrary luaLibrary)
        {
            Logger = logger;
            Subscription = subscription;
            EventHandlerController = eventHandlerController;
            EventBus = eventBus;
            InternalEventFactory = internalEventFactory;
            LuaLibrary = luaLibrary;
            WebWidgetEventFactory = webWidgetEventFactory;

            EventsServerModule = new WebSocketsEventsServer(Logger, Instances, eventBus, webWidgetEventFactory);

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

            var endpoint = $"{Url}/instances/{instanceId}";

            // Crude sanity check
            var indexFile = WEB_WIDGET_ROOT_DIRECTORY + webWidgetType + "/index.html";
            if (System.IO.File.Exists(indexFile))
            {
                EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceAdded(BroadcastEnvelope, "webwidget", instanceId));
                EventBus.PublishEvent(WebWidgetEventFactory.CreateWebWidgetEndpointAdded(BroadcastEnvelope, endpoint));

                Instances.Add(instanceId, webWidgetType, data);
                Subscription.AddImpersonate(instanceId);

                Logger.Information($"HttpServer: {endpoint} added");
            }
            else
            {
                Logger.Error($"HttpServer: {endpoint} not added, as {indexFile} does not exist");
            }
        }

        private void ThreadMain()
        {
            Logger.Information("HttpServer started");

            Stopping = false;

            using (var server = CreateWebServer(Url))
            {
                Task serverTask = server.RunAsync();

                var internalHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
                var webWidgetHandler = EventHandlerController.Get<WebWidgetEventHandler>();

                internalHandler.OnInternalCommandShutdown += (_, e) => Stopping = true;
                internalHandler.OnInternalDependencyAdded += (_, e) =>
                {
                    if (Instances.TryGetValue(e.DependsOn, out IWebWidgetInstances.Instance instance))
                    {
                        instance.Envelope = instance.Envelope.Add(e.InstanceId);
                    }
                };
                internalHandler.OnInternalDependencyRemoved += (_, e) =>
                {
                    if (Instances.TryGetValue(e.DependsOn, out IWebWidgetInstances.Instance instance))
                    {
                        instance.Envelope = instance.Envelope.Remove(e.InstanceId);

                        if (instance.Envelope.Recipients == null || instance.Envelope.Recipients.Length == 0)
                        {
                            InactiveInstance(e.DependsOn);
                        }
                    }
                };
                webWidgetHandler.OnWebWidgetCommandEvent += (_, e) =>
                {
                    if (e.Envelope.Recipients == null)
                        return;

                    foreach (var recipient in e.Envelope.Recipients)
                    {
                        EventsServerModule.Broadcast(recipient, e.Data);
                    }
                };

                while (!Stopping)
                {
                    IEvent? @event = Subscription.NextEvent(100);
                    EventHandlerController.HandleEvent(@event);

                    Stopping = Stopping || serverTask.IsCompleted;
                }
            }

            Logger.Information("HttpServer stopped");

            ServiceThread = null;
        }

        private void InactiveInstance(string instanceId)
        {
            RemoveInstance(instanceId);
        }

        public void RemoveInstance(string instanceId)
        {
            var endpoint = $"{Url}/instances/{instanceId}";
            Instances.Remove(instanceId);
            Subscription.DeleteImpersonation(instanceId);
            Logger.Information($"HttpServer: {endpoint} removed");
            EventBus.PublishEvent(WebWidgetEventFactory.CreateWebWidgetEndpointRemoved(BroadcastEnvelope, endpoint));
            EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceRemoved(BroadcastEnvelope, "webwidget", instanceId));
            Stopping = Instances.Count == 0;
            LuaLibrary.InstanceDropped(instanceId);
        }

        private EmbedIO.WebServer CreateWebServer(string url)
        {
            // endpoints:
            // /
            // /ss.js
            // /instances/<instanceid>/
            // /webwidgets/<webwidgettype>/
            // /events/<instanceid>/

            var server = new EmbedIO.WebServer(o => o
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

        public bool ContainsInstance(string instanceId)
        {
            return Instances.TryGetValue(instanceId, out _);
        }
    }
}