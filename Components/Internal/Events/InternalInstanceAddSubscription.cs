#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalInstanceAddSubscription : IEvent
    {
        public string EventType => nameof(InternalInstanceAddSubscription);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string InstanceId { get; set; } = string.Empty;
    }
}