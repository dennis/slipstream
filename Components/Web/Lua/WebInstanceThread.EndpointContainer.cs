#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private class EndpointContainer : IEndpointContainer
        {
            public IEndpointDefinition EndpointDefinition { get; set; }
            public List<string> Users { get; set; } = new List<string>();

            public EndpointContainer(IEndpointDefinition endpoint)
            {
                EndpointDefinition = endpoint;
            }
        }
    }
}