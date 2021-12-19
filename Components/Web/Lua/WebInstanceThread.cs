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
using System.Diagnostics;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread : BaseInstanceThread, IWebInstanceThread
    {
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IWebEventFactory WebEventFactory;
        private readonly string Url;
        private readonly Dictionary<string, IEndpointContainer> EndpointDefinitions = new Dictionary<string, IEndpointContainer>();
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
            // TODO: If we get multiple content for same endpoint, store them, but use the latest, so we can
            // restore the earlier version of the later one is removed
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
                {
                    RebuildWebServer = true;

                    // Check if any of our endpoints is only used by the removed dependency,
                    // and remove it if not used
                    lock (EndpointDefinitions)
                    {
                        var routeDeletionList = new List<string>();

                        foreach (var container in EndpointDefinitions)
                        {
                            if (container.Value.Users.Contains(e.Envelope.Sender))
                            {
                                container.Value.Users.RemoveAll(u => u == e.Envelope.Sender);

                                if (container.Value.Users.Count == 0)
                                {
                                    routeDeletionList.Add(container.Key);
                                }
                            }
                        }

                        foreach (var route in routeDeletionList)
                        {
                            RemoveEndpoint_EndpointDefinitionsNeedsToBeLocked(route);
                        }
                    }
                }
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
                using WebServer server = BuildWebServer();
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

            lock (EndpointDefinitions)
            {
                foreach (var route in EndpointDefinitions.Keys)
                {
                    RemoveEndpoint_EndpointDefinitionsNeedsToBeLocked(route);
                }
            }

            EventBus.PublishEvent(WebEventFactory.CreateWebServerRemoved(BroadcastEnvelope, Url));

            RebuildWebServer = true;

            Logger.Information($"Web: Stopping webserver for {Url}");
        }

        private WebServer BuildWebServer()
        {
            var server = new WebServer(o => o.WithUrlPrefix(Url));
            WebServerModules.Clear();

            lock (EndpointDefinitions)
            {
                foreach (var container in EndpointDefinitions.OrderByDescending(x => x.Key.Length))
                {
                    container.Value.EndpointDefinition.Apply(server, WebServerModules);
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
                AddEndpoint(e.Envelope.Sender, new WebModuleEndpoint(Url + e.Route, e.Route, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, content)));
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
            AddEndpoint(e.Envelope.Sender, new WebModuleEndpoint(Url.Replace("http://", "ws://") + e.Route, e.Route, new WebSocketsServerModule(EventBus, WebEventFactory, InstanceEnvelope, Logger, e.Route)));
        }

        private void OnWebCommandRoutePath(object? sender, Events.WebCommandRoutePath e)
        {
            AddEndpoint(e.Envelope.Sender, new StaticFolderEndpoint(Url + e.Route, e.Route, e.Path));
        }

        private void OnWebCommandRouteStaticContent(object? _, Events.WebCommandRouteStaticContent e)
        {
            AddEndpoint(e.Envelope.Sender, new WebModuleEndpoint(Url + e.Route, e.Route, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, e.Content)));
        }

        private void AddEndpoint(string senderInstanceId, IEndpointDefinition endpoint)
        {
            Logger.Information($"Endpoint: {senderInstanceId} added {endpoint.Route}");
            lock (EndpointDefinitions)
            {
                IEndpointContainer? currentEndpoint;

                var old = EndpointDefinitions.Where(a => a.Value.EndpointDefinition.Route == endpoint.Route).ToList();
                if (old != null && old.Count == 1)
                {
                    Logger.Information($"Web: Updating endpoint for {endpoint.Url}");
                    currentEndpoint = old[0].Value;
                    currentEndpoint.EndpointDefinition = endpoint;
                }
                else
                {
                    currentEndpoint = new EndpointContainer(endpoint);
                    Logger.Information($"Web: Adding new endpoint for {endpoint.Url}");
                    EndpointDefinitions.Add(endpoint.Route, currentEndpoint);
                    EventBus.PublishEvent(WebEventFactory.CreateWebEndpointAdded(InstanceEnvelope, endpoint.Route, endpoint.Url));
                }

                Debug.Assert(currentEndpoint != null);

                currentEndpoint.Users.Add(senderInstanceId);
            }

            RebuildWebServer = true;
        }

        private void RemoveEndpoint_EndpointDefinitionsNeedsToBeLocked(string route)
        {
            Logger.Information($"Endpoint: removed {route}");
            var endpoint = EndpointDefinitions[route];
            Logger.Information($"Web: Removing endpoint for {endpoint.EndpointDefinition.Url}");
            EndpointDefinitions.Remove(endpoint.EndpointDefinition.Route);
            EventBus.PublishEvent(WebEventFactory.CreateWebEndpointRemoved(InstanceEnvelope, endpoint.EndpointDefinition.Route, endpoint.EndpointDefinition.Url));
        }
    }
}