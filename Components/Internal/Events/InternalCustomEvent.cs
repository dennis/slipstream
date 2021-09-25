#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalCustomEvent : IEvent
    {
        public string EventType => nameof(InternalCustomEvent);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Name { get; set; } = string.Empty;

        public string Json { get; set; } = string.Empty;
    }
}