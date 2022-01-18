#nullable enable

using Slipstream;
using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerServerRemoved : IEvent
    {
        public string EventType => typeof(WebServerServerRemoved).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Server { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }
}