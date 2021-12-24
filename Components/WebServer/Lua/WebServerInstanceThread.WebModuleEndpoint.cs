#nullable enable

using System.Collections.Generic;

using EmbedIO;

namespace Slipstream.Components.WebServer.Lua
{
    public partial class WebServerInstanceThread
    {
        private class WebModuleEndpoint : IEndpointDefinition
        {
            private readonly IWebModule WebModule;

            public string Creator { get; private set; }

            public WebModuleEndpoint(string creator, IWebModule webModule)
            {
                Creator = creator;
                WebModule = webModule;
            }

            public void Apply(EmbedIO.WebServer ws, string route, Dictionary<string, IWebModule> webServerModules)
            {
                ws.WithModule(WebModule);
                webServerModules.Add(route, WebModule);
            }
        }
    }
}