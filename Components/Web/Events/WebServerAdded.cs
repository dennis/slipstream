#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebServerAdded : IEvent
    {
        public string EventType => typeof(WebServerAdded).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
    }
}