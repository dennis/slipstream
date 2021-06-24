using Slipstream.Components.Internal.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal.EventFactory
{
    public class InternalEventFactory : IInternalEventFactory
    {
        public InternalCommandShutdown CreateInternalCommandShutdown(IEventEnvelope envelope)
        {
            return new InternalCommandShutdown { Envelope = envelope };
        }

        public InternalInstanceAddSubscription CreateInternalInstanceAddSubscription(IEventEnvelope envelope, string instanceId)
        {
            return new InternalInstanceAddSubscription
            {
                Envelope = envelope,
                InstanceId = instanceId,
            };
        }

        public InternalInstanceRemoveSubscription CreateInternalInstanceRemoveSubscription(IEventEnvelope envelope, string instanceId)
        {
            return new InternalInstanceRemoveSubscription
            {
                Envelope = envelope,
                InstanceId = instanceId,
            };
        }
    }
}