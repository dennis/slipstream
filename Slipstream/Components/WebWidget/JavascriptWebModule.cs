#nullable enable

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;

namespace Slipstream.Components.WebWidget
{
    public class JavascriptWebModule : IWebModule
    {
        private readonly RouteMatcher RouteMatcher;
        private readonly string Content;
        public bool IsFinalHandler => true;
        public ExceptionHandlerCallback? OnUnhandledException { get; set; }
        public HttpExceptionHandlerCallback? OnHttpException { get; set; }
        public string BaseRoute => "/ss.js";

        public JavascriptWebModule()
        {
            RouteMatcher = RouteMatcher.Parse(BaseRoute, true);

            var assembly = GetType().Assembly;
            using var s = assembly.GetManifestResourceStream("Slipstream.Backend.WebWidget.ss.js");
            if (s != null)
            {
                using var sr = new StreamReader(s);
                Content = sr.ReadToEnd();
            }
            else
            {
                throw new Exception("Resource not available: Slipstream.Backend.WebWidget.ss.js");
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

            return context.SendStringAsync(Content, "application/javascript", Encoding.UTF8);
        }
    }
}