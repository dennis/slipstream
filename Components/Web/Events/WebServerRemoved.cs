#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebServerRemoved : IEvent
    {
        public string EventType => typeof(WebServerRemoved).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Endpoint { get; set; } = string.Empty;
    }
}