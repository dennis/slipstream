#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerCommandRoutePath : IEvent
    {
        public string EventType => typeof(WebServerCommandRoutePath).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string Path { get; set; } = "";
    }
}