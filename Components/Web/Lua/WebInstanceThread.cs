#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using EmbedIO;

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Components.Web.EventHandler;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread : BaseInstanceThread, IWebInstanceThread
    {
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IWebEventFactory WebEventFactory;
        private readonly string Url;
        private readonly Dictionary<string, IEndpointDefinition> EndpointDefinitions = new Dictionary<string, IEndpointDefinition>();
        private readonly Dictionary<string, IWebModule> WebServerModules = new Dictionary<string, IWebModule>();
        private volatile bool RebuildWebServer = true;

        public WebInstanceThread(
            string luaLibraryName,
            string instanceId,
            long port,
            IEventBusSubscription eventBusSubscription,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IInternalEventFactory internalEventFactory,
            IWebEventFactory webEventFactory,
            ILogger logger) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            WebEventFactory = webEventFactory;

            Url = $"http://127.0.0.1:{port}"; // Must NOT end with slash
        }

        protected override void Main()
        {
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();

            // We need to reconfigure the webserver every time the Envelope is updated by BaseInstanceThreadclass, so
            // the events sent by the webserver will use the correct Envelope
            internalEventHandler.OnInternalDependencyAdded += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                    RebuildWebServer = true;
            };
            internalEventHandler.OnInternalDependencyRemoved += (_, e) =>
            {
                if (e.DependsOn == InstanceId)
                    RebuildWebServer = true;
            };

            var webEventHandler = EventHandlerController.Get<WebEventHandler>();
            webEventHandler.OnWebCommandRouteStaticContent += OnWebCommandRouteStaticContent;
            webEventHandler.OnWebCommandRoutePath += OnWebCommandRoutePath;
            webEventHandler.OnWebCommandRouteWebSocket += OnWebCommandRouteWebSocket;
            webEventHandler.OnWebCommandData += OnWebCommandData;
            webEventHandler.OnWebCommandRouteFileContent += OnWebCommandRouteFileContent;

            Logger.Information($"Web: Starting webserver for {Url}");

            EventBus.PublishEvent(WebEventFactory.CreateWebServerAdded(BroadcastEnvelope, Url));

            while (!Stopping)
            {
                WebServer server = BuildWebServer();
                RebuildWebServer = false;

                while (!Stopping && !RebuildWebServer)
                {
                    IEvent? @event = Subscription.NextEvent(100);

                    if (@event != null)
                    {
                        EventHandlerController.HandleEvent(@event);
                    }
                }
            }

            EventBus.PublishEvent(WebEventFactory.CreateWebServerRemoved(BroadcastEnvelope, Url));

            foreach (var e in EndpointDefinitions.Values)
            {
                RemoveEndpoint(e);
            }

            Logger.Information($"Web: Stopping webserver for {Url}");
        }

        private WebServer BuildWebServer()
        {
            var server = new WebServer(o => o.WithUrlPrefix(Url));
            WebServerModules.Clear();

            lock (EndpointDefinitions)
            {
                foreach (var item in EndpointDefinitions.OrderByDescending(x => x.Key.Length))
                {
                    item.Value.Apply(server, WebServerModules);
                }

                server.HandleHttpException(async (ctx, ex) =>
                {
                    ctx.Response.StatusCode = ex.StatusCode;
                    await ctx.SendStringAsync($"Slipstream webserver returned {ex.StatusCode}", "text/plain", Encoding.UTF8);
                });

                Task serverTask = server.RunAsync();

                return server;
            }
        }

        private void OnWebCommandRouteFileContent(object? sender, Events.WebCommandRouteFileContent e)
        {
            try
            {
                string content = File.ReadAllText(e.Filename);
                AddEndpoint(new WebModuleEndpoint(Url + e.Route, e.Route, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, content)));
            }
            catch (Exception ex)
            {
                Logger.Error($"Web: Error reading file-content for {e.Filename}: {ex.Message}");
            }
        }

        private void OnWebCommandData(object? sender, Events.WebCommandData e)
        {
            lock (EndpointDefinitions)
            {
                if (WebServerModules.TryGetValue(e.Route, out IWebModule? value))
                {
                    if (value is WebSocketsServerModule module)
                    {
                        module.SendMessage(e.ClientId, e.Data);
                    }
                    else
                    {
                        Logger.Warning($"Web: Data sent to non websocket route '{e.Route}': {e.Data}");
                    }
                }
                else
                {
                    Logger.Warning($"Web: Data sent to non existing route '{e.Route}': {e.Data}");
                }
            }
        }

        private void OnWebCommandRouteWebSocket(object? sender, Events.WebCommandRouteWebSocket e)
        {
            AddEndpoint(new WebModuleEndpoint(Url.Replace("http://", "ws://") + e.Route, e.Route, new WebSocketsServerModule(EventBus, WebEventFactory, InstanceEnvelope, Logger, e.Route)));
        }

        private void OnWebCommandRoutePath(object? sender, Events.WebCommandRoutePath e)
        {
            AddEndpoint(new StaticFolderEndpoint(Url + e.Route, e.Route, e.Path));
        }

        private void OnWebCommandRouteStaticContent(object? _, Events.WebCommandRouteStaticContent e)
        {
            AddEndpoint(new WebModuleEndpoint(Url + e.Route, e.Route, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, e.Content)));
        }

        private void AddEndpoint(IEndpointDefinition endpoint)
        {
            RemoveEndpoint(endpoint); // Make sure it's gone

            lock (EndpointDefinitions)
            {
                Logger.Information($"Web: Adding endpoint for {endpoint.Url}");
                EndpointDefinitions.Add(endpoint.Route, endpoint);
            }

            RebuildWebServer = true;

            EventBus.PublishEvent(WebEventFactory.CreateWebEndpointAdded(InstanceEnvelope, $"{endpoint.Url}"));
        }

        private void RemoveEndpoint(IEndpointDefinition endpoint)
        {
            lock (EndpointDefinitions)
            {
                if (EndpointDefinitions.ContainsKey(endpoint.Route))
                {
                    Logger.Information($"Web: Removing endpoint for {endpoint.Url}");
                    EndpointDefinitions.Remove(endpoint.Route);
                    EventBus.PublishEvent(WebEventFactory.CreateWebEndpointRemoved(InstanceEnvelope, $"{endpoint.Url}"));
                }
            }

            RebuildWebServer = true;
        }
    }
}