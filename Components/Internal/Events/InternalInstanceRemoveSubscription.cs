#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalInstanceRemoveSubscription : IEvent
    {
        public string EventType => nameof(InternalInstanceRemoveSubscription);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string InstanceId { get; set; } = string.Empty;
    }
}