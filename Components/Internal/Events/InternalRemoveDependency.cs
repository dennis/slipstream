#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalRemoveDependency : IEvent
    {
        public string EventType => nameof(InternalRemoveDependency);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string InstanceId { get; set; } = string.Empty;
    }
}