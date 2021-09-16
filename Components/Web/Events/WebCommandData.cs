#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Web.Events
{
    public class WebCommandData : IEvent
    {
        public string EventType => typeof(WebCommandData).Name;
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Route { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty; // if empty then it's broadcast
        public string Data { get; set; } = string.Empty;
    }
}