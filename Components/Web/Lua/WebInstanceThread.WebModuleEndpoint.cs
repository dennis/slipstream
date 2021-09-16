#nullable enable

using System.Collections.Generic;

using EmbedIO;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class WebModuleEndpoint : IEndpointDefinition
        {
            private readonly IWebModule WebModule;

            public string Route { get; private set; }

            public string Url { get; private set; }

            public WebModuleEndpoint(string url, string route, IWebModule webModule)
            {
                Route = route;
                WebModule = webModule;
                Url = url;
            }

            public void Apply(WebServer ws, Dictionary<string, IWebModule> webServerModules)
            {
                ws.WithModule(WebModule);
                webServerModules.Add(Route, WebModule);
            }
        }
    }
}