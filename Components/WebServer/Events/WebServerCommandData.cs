#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WebServer.Events
{
    public class WebServerCommandData : IEvent
    {
        public string EventType => typeof(WebServerCommandData).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty; // if empty then it's broadcast
        public string Data { get; set; } = string.Empty;
    }
}