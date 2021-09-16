#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebCommandRouteFileContent : IEvent
    {
        public string EventType => typeof(WebCommandRouteFileContent).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string MimeType { get; set; } = "text/plain";
        public string Filename { get; set; } = "";
    }
}