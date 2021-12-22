#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerCommandRouteStaticContent : IEvent
    {
        public string EventType => typeof(WebServerCommandRouteStaticContent).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string MimeType { get; set; } = "text/plain";
        public string Content { get; set; } = "Hello world";
    }
}