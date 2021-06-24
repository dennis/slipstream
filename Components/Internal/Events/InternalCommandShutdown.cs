#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalCommandShutdown : IEvent
    {
        public string EventType => nameof(InternalCommandShutdown);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}