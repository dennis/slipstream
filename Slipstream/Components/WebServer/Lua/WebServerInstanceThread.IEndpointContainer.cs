#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.WebServer.Lua
{
    public partial class WebServerInstanceThread
    {
        private interface IEndpointContainer
        {
            string Route { get; }
            string Url { get; }

            List<string> Users { get; set; }
            List<IEndpointDefinition> EndpointDefinitions { get; set; }
        }
    }
}