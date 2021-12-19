#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.Web.Lua
{
    public partial class WebInstanceThread
    {
        private interface IEndpointContainer
        {
            IEndpointDefinition EndpointDefinition { get; set; }
            List<string> Users { get; set; }
        }
    }
}