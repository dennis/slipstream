#nullable enable

using Slipstream;
using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerCommandRouteWebSocket : IEvent
    {
        public string EventType => typeof(WebServerCommandRouteWebSocket).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
    }
}