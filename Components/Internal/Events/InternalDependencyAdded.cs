#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Internal.Events
{
    public class InternalDependencyAdded : IEvent
    {
        public string EventType => nameof(InternalDependencyAdded);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string LuaLibrary { get; set; } = string.Empty;

        public string InstanceId { get; set; } = string.Empty;

        public string DependsOn { get; set; } = string.Empty;
    }
}