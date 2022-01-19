#nullable enable

using EmbedIO;

namespace Slipstream.Components.WebServer.Lua
{
    public partial class WebServerInstanceThread
    {
        private interface IEndpointDefinition
        {
            public string Creator { get; }

            void Apply(EmbedIO.WebServer ws, string route, System.Collections.Generic.Dictionary<string, IWebModule> webServerModules);
        }
    }
}