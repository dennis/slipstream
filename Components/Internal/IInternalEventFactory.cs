using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IInternalEventFactory
    {
        InternalCommandShutdown CreateInternalCommandShutdown(IEventEnvelope envelope);

        InternalDependencyAdded CreateInternalDependencyAdded(IEventEnvelope envelope, string luaLibrary, string instanceId, string dependsOn);

        InternalDependencyRemoved CreateInternalDependencyRemoved(IEventEnvelope envelope, string luaLibrary, string instanceId, string dependsOn);

        InternalInstanceAdded CreateInternalInstanceAdded(IEventEnvelope envelope, string luaLibrary, string instanceId);

        InternalInstanceRemoved CreateInternalInstanceRemoved(IEventEnvelope envelope, string luaLibrary, string instanceId);
    }
}