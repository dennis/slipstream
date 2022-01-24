#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerCommandRouteFileContent : IEvent
    {
        public string EventType => typeof(WebServerCommandRouteFileContent).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string MimeType { get; set; } = "text/plain";
        public string Filename { get; set; } = "";
    }
}