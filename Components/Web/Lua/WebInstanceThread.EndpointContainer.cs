#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class EndpointContainer : IEndpointContainer
        {
            public string Creator { get; private set; }
            public List<string> Users { get; set; } = new List<string>();
            public List<IEndpointDefinition> EndpointDefinitions { get; set; } = new List<IEndpointDefinition>();
            public string Route { get; private set; }

            public string Url { get; private set; }

            public EndpointContainer(string creator, string route, string url)
            {
                Creator = creator;
                Route = route;
                Url = url;
            }
        }
    }
}