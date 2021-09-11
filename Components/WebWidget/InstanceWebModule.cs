#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;

using Serilog;

namespace Slipstream.Components.WebWidget
{
    public class InstanceWebModule : IWebModule
    {
        private readonly RouteMatcher RouteMatcher;
        private readonly ILogger Logger;
        private readonly IWebWidgetInstances Instances;
        private readonly string WebWidgetDirectory;
        public bool IsFinalHandler => true;
        public ExceptionHandlerCallback? OnUnhandledException { get; set; }
        public HttpExceptionHandlerCallback? OnHttpException { get; set; }
        public string BaseRoute => "/instances/{id}";

        public InstanceWebModule(IWebWidgetInstances instances, string webWidgetDirectory, ILogger logger)
        {
            Instances = instances;
            WebWidgetDirectory = webWidgetDirectory;
            RouteMatcher = RouteMatcher.Parse(BaseRoute, true);
            Logger = logger;
        }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

        public RouteMatch? MatchUrlPath(string urlPath)
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            return RouteMatcher.Match(urlPath);
        }

        public void Start(CancellationToken cancellationToken)
        {
        }

        public Task HandleRequestAsync(IHttpContext context)
        {
            try
            {
                var instanceId = context.Route["id"];
                var webWidgetType = Instances[instanceId].Type;

                var template = File.ReadAllText(WebWidgetDirectory + webWidgetType + "/index.html");
                var assets = "/webwidgets/" + webWidgetType;
                var rendered = template
                    .Replace("{{ASSETS}}", assets)
                    .Replace("{{SLIPSTREAM_BODY_ATTRS}}", $" data-instance-id=\"{instanceId}\" data-web-widget-type=\"{webWidgetType}\" data-assets=\"{assets}\"")
                    .Replace("{{SLIPSTREAM_HEADERS}}", "<script type=\"text/javascript\" src =\"/ss.js\"></script>");

                context.SetHandled();
                context.Response.Headers.Add(HttpHeaderNames.CacheControl, "no-cache");
                return context.SendStringAsync(rendered, MimeType.Html, Encoding.UTF8);
            }
            catch (KeyNotFoundException)
            {
                context.Response.StatusCode = 404;
                context.SetHandled();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }
    }
}