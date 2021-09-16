#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebCommandRouteWebSocket : IEvent
    {
        public string EventType => typeof(WebCommandRouteWebSocket).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
    }
}