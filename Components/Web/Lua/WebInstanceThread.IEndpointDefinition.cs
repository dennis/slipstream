#nullable enable

using EmbedIO;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private interface IEndpointDefinition
        {
            public string Route { get; }
            public string Url { get; }

            void Apply(WebServer ws, System.Collections.Generic.Dictionary<string, IWebModule> webServerModules);
        }
    }
}