#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebCommandRoutePath : IEvent
    {
        public string EventType => typeof(WebCommandRoutePath).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string Path { get; set; } = "";
    }
}