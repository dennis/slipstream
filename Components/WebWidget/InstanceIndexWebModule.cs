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
    public class InstanceIndexWebModule : IWebModule
    {
        private readonly RouteMatcher RouteMatcher;
        private readonly ILogger Logger;
        private readonly IWebWidgetInstances Instances;
        private readonly string Template;
        public bool IsFinalHandler => true;
        public ExceptionHandlerCallback? OnUnhandledException { get; set; }
        public HttpExceptionHandlerCallback? OnHttpException { get; set; }
        public string BaseRoute => "/";

        public InstanceIndexWebModule(IWebWidgetInstances instances, ILogger logger)
        {
            Instances = instances;
            RouteMatcher = RouteMatcher.Parse(BaseRoute, true);
            Logger = logger;

            var assembly = GetType().Assembly;
            using var s = assembly.GetManifestResourceStream("Slipstream.Backend.WebWidget.InstanceIndex.html");
            if (s != null)
            {
                using var sr = new StreamReader(s);
                Template = sr.ReadToEnd();
            }
            else
            {
                throw new Exception("Resource not available: Slipstream.Backend.WebWidget.InstanceIndex.html");
            }
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
            if (context.IsHandled)
                return Task.CompletedTask;

            try
            {
                var content = "";

                if (Instances.GetIds().Count == 0)
                {
                    content = Template.Replace("{{CONTENT}}", "No instances");
                }
                else
                {
                    var instancesBlock = "";

                    foreach (var instanceId in Instances.GetIds())
                    {
                        instancesBlock += $"<li><a href=\"/instances/{instanceId}\">{instanceId}</a></li>";
                    }

                    content = Template.Replace("{{CONTENT}}", $"<ul>{instancesBlock}</ul>");
                }

                context.Response.Headers.Add(HttpHeaderNames.CacheControl, "no-cache");
                return context.SendStringAsync(content, MimeType.Html, Encoding.UTF8);
            }
            catch (KeyNotFoundException)
            {
                context.Response.StatusCode = 404;
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