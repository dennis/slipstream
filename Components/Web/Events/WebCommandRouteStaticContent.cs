#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebCommandRouteStaticContent : IEvent
    {
        public string EventType => typeof(WebCommandRouteStaticContent).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = "/";
        public string MimeType { get; set; } = "text/plain";
        public string Content { get; set; } = "Hello world";
    }
}