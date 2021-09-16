#nullable enable

using System.Collections.Generic;

using EmbedIO;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class StaticFolderEndpoint : IEndpointDefinition
        {
            private readonly string Path;
            public string Url { get; private set; }
            public string Route { get; private set; }

            public StaticFolderEndpoint(string url, string route, string path)
            {
                Route = route;
                Path = path;
                Url = url;
            }

            public void Apply(WebServer ws, Dictionary<string, IWebModule> webServerModules)
            {
                ws.WithStaticFolder(Route, Path, false);
            }
        }
    }
}