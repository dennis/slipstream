#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
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