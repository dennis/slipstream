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
using Slipstream.Components.WebServer.EventHandler;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System.Diagnostics;
using Slipstream.Components.Internal.Events;
using Slipstream.Components.WebServer.Events;

namespace Slipstream.Components.WebServer.Lua
{
    public partial class WebServerInstanceThread : BaseInstanceThread, IWebServerInstanceThread
    {
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IWebServerEventFactory WebEventFactory;
        private readonly string Url;
        private readonly Dictionary<string, IEndpointContainer> EndpointContainers = new Dictionary<string, IEndpointContainer>();
        private readonly Dictionary<string, IWebModule> WebServerModules = new Dictionary<string, IWebModule>();
        private volatile bool RebuildWebServer = true;

        public WebServerInstanceThread(
            string luaLibraryName,
            string instanceId,
            long port,
            IEventBusSubscription eventBusSubscription,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IInternalEventFactory internalEventFactory,
            IWebServerEventFactory webEventFactory,
            ILogger logger) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            WebEventFactory = webEventFactory;

            Url = $"http://*:{port}"; // Must NOT end with slash
        }

        protected override void Main()
        {
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();

            // We need to reconfigure the webserver every time the Envelope is updated by BaseInstanceThreadclass, so
            // the events sent by the webserver will use the correct Envelope
            internalEventHandler.OnInternalDependencyAdded += (_, e) => OnInternalDependencyAdded(e);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => OnInternalDependencyRemoved(e);

            var webEventHandler = EventHandlerController.Get<WebServerEventHandler>();
            webEventHandler.OnWebServerCommandRouteStaticContent += OnWebServerCommandRouteStaticContent;
            webEventHandler.OnWebServerCommandRoutePath += OnWebServerCommandRoutePath;
            webEventHandler.OnWebServerCommandRouteWebSocket += OnWebServerCommandRouteWebSocket;
            webEventHandler.OnWebServerCommandData += OnWebServerCommandData;
            webEventHandler.OnWebServerCommandRouteFileContent += OnWebServerCommandRouteFileContent;

            Logger.Information($"Web: Starting webserver for {Url}");

            EventBus.PublishEvent(WebEventFactory.CreateWebServerServerAdded(BroadcastEnvelope, Url));

            while (!Stopping)
            {
                using EmbedIO.WebServer server = BuildWebServer();
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

            Shutdown();
        }

        private void Shutdown()
        {
            lock (EndpointContainers)
            {
                foreach (var (_, container) in EndpointContainers)
                {
                    for (int i = 0; i < container.EndpointDefinitions.Count; i++)
                    {
                        RemoveEndpoint_EndpointDefinitionsNeedsToBeLocked(
                            container.Route,
                            container.Url,
                            container.EndpointDefinitions[i].Creator
                        );
                    }
                }
            }

            EventBus.PublishEvent(WebEventFactory.CreateWebServerServerRemoved(BroadcastEnvelope, Url));

            RebuildWebServer = true;

            Logger.Information($"Web: Stopping webserver for {Url}");
        }

        private void OnInternalDependencyRemoved(InternalDependencyRemoved e)
        {
            if (e.DependsOn == InstanceId)
            {
                RebuildWebServer = true;

                // Check if any of our endpoints is only used by the removed dependency,
                // and remove it if not used
                lock (EndpointContainers)
                {
                    var containerDeletionList = new List<string>();
                    var endpointDeletionList = new List<Tuple<IEndpointContainer, IEndpointDefinition>>();

                    foreach (var (route, container) in EndpointContainers)
                    {
                        container.Users.RemoveAll(u => u == e.Envelope.Sender);

                        if (container.Users.Count == 0)
                        {
                            // Nobody uses this anymore, let's remove it
                            if (!containerDeletionList.Contains(route))
                                containerDeletionList.Add(route);
                        }
                        else
                        {
                            foreach (var item in container.EndpointDefinitions)
                            {
                                if (item.Creator == e.Envelope.Sender)
                                {
                                    if (container.EndpointDefinitions.Count == 1)
                                    {
                                        if (!containerDeletionList.Contains(route))
                                            containerDeletionList.Add(route);
                                    }
                                    else
                                    {
                                        endpointDeletionList.Add(new Tuple<IEndpointContainer, IEndpointDefinition>(container, item));
                                    }
                                }
                            }
                        }
                    }

                    // Remove unused endpoints. There are more endpoint definitions, so we don't need to announce the
                    // removal of it as something else will take over
                    foreach (var (container, item) in endpointDeletionList)
                    {
                        container.EndpointDefinitions.Remove(item);
                    }

                    // Remove containers by first removing the endpoints and then the container
                    foreach (var key in containerDeletionList)
                    {
                        var route = EndpointContainers[key].Route;
                        var url = EndpointContainers[key].Url;

                        foreach (var item in EndpointContainers[route].EndpointDefinitions)
                        {
                            Logger.Information($"Web: Removing endpoint for '{route}'. Shutting it down, as nobody is using it");
                            EventBus.PublishEvent(WebEventFactory.CreateWebServerEndpointRemoved(InstanceEnvelope, route, url));
                        }

                        EndpointContainers.Remove(route);
                    }
                }
            }
        }

        private void OnInternalDependencyAdded(InternalDependencyAdded e)
        {
            if (e.DependsOn == InstanceId)
                RebuildWebServer = true;
        }

        private EmbedIO.WebServer BuildWebServer()
        {
            var server = new EmbedIO.WebServer(o => o.WithUrlPrefix(Url));
            WebServerModules.Clear();

            lock (EndpointContainers)
            {
                foreach (var (_, container) in EndpointContainers.OrderByDescending(x => x.Key.Length))
                {
                    container.EndpointDefinitions.Last().Apply(server, container.Route, WebServerModules);
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

        private void OnWebServerCommandRouteFileContent(object? sender, Events.WebServerCommandRouteFileContent e)
        {
            try
            {
                string content = File.ReadAllText(e.Filename);
                AddEndpoint(e.Envelope.Sender, e.Route, Url + e.Route, new WebModuleEndpoint(e.Envelope.Sender, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, content)));
            }
            catch (Exception ex)
            {
                Logger.Error($"Web: Error reading file-content for {e.Filename}: {ex.Message}");
            }
        }

        private void OnWebServerCommandData(object? sender, Events.WebServerCommandData e)
        {
            lock (EndpointContainers)
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

        private void OnWebServerCommandRouteWebSocket(object? sender, WebServerCommandRouteWebSocket e)
        {
            AddEndpoint(e.Envelope.Sender, e.Route, Url.Replace("http://", "ws://") + e.Route, new WebModuleEndpoint(e.Envelope.Sender, new WebSocketsServerModule(EventBus, WebEventFactory, InstanceEnvelope, Logger, e.Route)));
        }

        private void OnWebServerCommandRoutePath(object? sender, Events.WebServerCommandRoutePath e)
        {
            AddEndpoint(e.Envelope.Sender, e.Route, Url + e.Route, new StaticFolderEndpoint(e.Envelope.Sender, e.Path));
        }

        private void OnWebServerCommandRouteStaticContent(object? _, Events.WebServerCommandRouteStaticContent e)
        {
            AddEndpoint(e.Envelope.Sender, e.Route, Url + e.Route, new WebModuleEndpoint(e.Envelope.Sender, new StaticContentServerModule(EventBus, WebEventFactory, InstanceEnvelope, e.Route, e.MimeType, e.Content)));
        }

        private void AddEndpoint(string senderInstanceId, string route, string url, IEndpointDefinition endpoint)
        {
            Logger.Information($"Endpoint: {senderInstanceId} added {route}");

            lock (EndpointContainers)
            {
                IEndpointContainer? current;

                if (EndpointContainers.TryGetValue(route, out IEndpointContainer? container) && container != null)
                {
                    Logger.Information($"Web: Updating endpoint for {url}");

                    // Try to find the definition created by the same instance and remove it
                    container.EndpointDefinitions.RemoveAll(a => a.Creator == senderInstanceId);
                    container.EndpointDefinitions.Add(endpoint);

                    current = container;
                }
                else
                {
                    current = new EndpointContainer(senderInstanceId, route, url);
                    current.EndpointDefinitions.Add(endpoint);

                    Logger.Information($"Web: Adding new endpoint for {url}");
                    EndpointContainers.Add(route, current);
                    EventBus.PublishEvent(WebEventFactory.CreateWebServerEndpointAdded(InstanceEnvelope, route, url));
                }

                Debug.Assert(current != null);

                current.Users.Add(senderInstanceId);
            }

            RebuildWebServer = true;
        }

        private void RemoveEndpoint_EndpointDefinitionsNeedsToBeLocked(string route, string url, string senderInstanceId)
        {
            if (EndpointContainers.TryGetValue(route, out IEndpointContainer? container) && container != null)
            {
                // Try to find the definition created by the same instance and remove it
                container.EndpointDefinitions.RemoveAll(a => a.Creator == senderInstanceId);
                container.Users.RemoveAll(a => a == senderInstanceId);

                if (container.Users.Count == 0 || container.EndpointDefinitions.Count == 0)
                {
                    // remove the last element, so that any other owners, creating this endpoint will be restored
                    Logger.Information($"Web: Removing endpoint for '{route}'. Shutting it down");
                    EventBus.PublishEvent(WebEventFactory.CreateWebServerEndpointRemoved(InstanceEnvelope, route, url));
                    EndpointContainers.Remove(route);

                    RebuildWebServer = true;
                }
                else
                {
                    EndpointContainers[route] = container;
                }
            }
        }
    }
}