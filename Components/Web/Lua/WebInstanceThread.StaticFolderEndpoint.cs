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
            public string Creator { get; private set; }

            public StaticFolderEndpoint(string creator, string path)
            {
                Creator = creator;
                Path = path;
            }

            public void Apply(WebServer ws, string route, Dictionary<string, IWebModule> webServerModules)
            {
                ws.WithStaticFolder(route, Path, false);
            }
        }
    }
}