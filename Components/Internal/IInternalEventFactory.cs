using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IInternalEventFactory
    {
        InternalCommandShutdown CreateInternalCommandShutdown(IEventEnvelope envelope);

        InternalAddDependency CreateInternalAddDependency(IEventEnvelope envelope, string instanceId);

        InternalRemoveDependency CreateInternalRemoveDependency(IEventEnvelope envelope, string instanceId);
    }
}