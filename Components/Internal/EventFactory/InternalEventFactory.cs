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

        public InternalAddDependency CreateInternalAddDependency(IEventEnvelope envelope, string instanceId)
        {
            return new InternalAddDependency
            {
                Envelope = envelope,
                InstanceId = instanceId,
            };
        }

        public InternalRemoveDependency CreateInternalRemoveDependency(IEventEnvelope envelope, string instanceId)
        {
            return new InternalRemoveDependency
            {
                Envelope = envelope,
                InstanceId = instanceId,
            };
        }
    }
}