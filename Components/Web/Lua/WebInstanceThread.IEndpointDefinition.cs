#nullable enable

using EmbedIO;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private interface IEndpointDefinition
        {
            public string Creator { get; }

            void Apply(WebServer ws, string route, System.Collections.Generic.Dictionary<string, IWebModule> webServerModules);
        }
    }
}