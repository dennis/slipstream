#nullable enable

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using EmbedIO;
using EmbedIO.Routing;

using Slipstream.Shared;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class StaticContentServerModule : IWebModule
        {
            private readonly RouteMatcher RouteMatcher;
            private readonly string Content;
            private readonly string MimeType;
            private readonly IEventBus EventBus;
            private readonly IWebEventFactory WebEventFactory;
            private readonly IEventEnvelope Envelope;

            public string BaseRoute { get; private set; }
            public bool IsFinalHandler => true;

            public ExceptionHandlerCallback? OnUnhandledException { get; set; }
            public HttpExceptionHandlerCallback? OnHttpException { get; set; }

            public StaticContentServerModule(
                IEventBus eventBus,
                IWebEventFactory webEventFactory,
                IEventEnvelope envelope,
                string route,
                string mineType,
                string content
            )
            {
                EventBus = eventBus;
                WebEventFactory = webEventFactory;
                Envelope = envelope;
                BaseRoute = route;
                RouteMatcher = RouteMatcher.Parse(BaseRoute, true);
                Content = content;
                MimeType = mineType;
            }

            public async Task HandleRequestAsync(IHttpContext context)
            {
                if (context.IsHandled)
                    return;

                var httpMethod = context.Request.HttpMethod;
                var httpBody = await context.GetRequestBodyAsStringAsync();
                var httpQueryParams = new List<string>();

                for (int idx = 0; idx < context.Request.QueryString.Count; idx++)
                {
                    var k = context.Request.QueryString.GetKey(idx);
                    var vals = context.Request.QueryString.GetValues(idx);

                    if (vals == null)
                    {
                        httpQueryParams.Add(HttpUtility.UrlEncode(k) + "=");
                    }
                    else
                    {
                        foreach (var v in vals)
                        {
                            httpQueryParams.Add(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(v));
                        }
                    }
                }

                EventBus.PublishEvent(WebEventFactory.CreateWebEndpointRequested(Envelope, BaseRoute, httpMethod, httpBody, string.Join("&", httpQueryParams)));

                await context.SendStringAsync(Content, MimeType, Encoding.UTF8);
            }

            public RouteMatch MatchUrlPath(string urlPath)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return RouteMatcher.Match(urlPath);
#pragma warning restore CS8603 // Possible null reference return.
            }

            public void Start(CancellationToken cancellationToken)
            {
            }
        }
    }
}