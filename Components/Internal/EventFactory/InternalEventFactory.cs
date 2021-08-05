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

        public InternalDependencyAdded CreateInternalDependencyAdded(IEventEnvelope envelope, string luaLibrary, string instanceId, string dependsOn)
        {
            return new InternalDependencyAdded
            {
                Envelope = envelope,
                LuaLibrary = luaLibrary,
                InstanceId = instanceId,
                DependsOn = dependsOn,
            };
        }

        public InternalDependencyRemoved CreateInternalDependencyRemoved(IEventEnvelope envelope, string luaLibrary, string instanceId, string dependsOn)
        {
            return new InternalDependencyRemoved
            {
                Envelope = envelope,
                LuaLibrary = luaLibrary,
                InstanceId = instanceId,
                DependsOn = dependsOn,
            };
        }

        public InternalInstanceAdded CreateInternalInstanceAdded(IEventEnvelope envelope, string luaLibrary, string instanceId)
        {
            return new InternalInstanceAdded
            {
                Envelope = envelope,
                LuaLibrary = luaLibrary,
                InstanceId = instanceId
            };
        }

        public InternalInstanceRemoved CreateInternalInstanceRemoved(IEventEnvelope envelope, string luaLibrary, string instanceId)
        {
            return new InternalInstanceRemoved
            {
                Envelope = envelope,
                LuaLibrary = luaLibrary,
                InstanceId = instanceId
            };
        }
    }
}