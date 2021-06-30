#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalAddDependency : IEvent
    {
        public string EventType => nameof(InternalAddDependency);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string InstanceId { get; set; } = string.Empty;
    }
}