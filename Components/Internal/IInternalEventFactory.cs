using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IInternalEventFactory
    {
        InternalCommandShutdown CreateInternalCommandShutdown(IEventEnvelope envelope);
        InternalInstanceAddSubscription CreateInternalInstanceAddSubscription(IEventEnvelope envelope, string instanceId);
        InternalInstanceRemoveSubscription CreateInternalInstanceRemoveSubscription(IEventEnvelope envelope, string instanceId);
    }
}