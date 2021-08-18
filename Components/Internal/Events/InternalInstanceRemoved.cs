#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalInstanceRemoved : IEvent
    {
        public string EventType => nameof(InternalInstanceRemoved);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string LuaLibrary { get; set; } = string.Empty;

        public string InstanceId { get; set; } = string.Empty;
    }
}