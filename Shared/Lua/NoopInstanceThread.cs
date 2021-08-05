#nullable enable

using System;

using Slipstream.Components.Internal;

namespace Slipstream.Shared.Lua
{
    public class NoopInstanceThread : ILuaInstanceThread
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly string LuaLibraryName;
        private readonly string InstanceId;
        private readonly IEventEnvelope Envelope;

        public NoopInstanceThread(string luaLibraryName, string instanceId, IEventBus eventBus, IInternalEventFactory internalEventFactory)
        {
            EventBus = eventBus;
            InternalEventFactory = internalEventFactory;
            LuaLibraryName = luaLibraryName;
            InstanceId = instanceId;
            Envelope = new EventEnvelope(instanceId);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceAdded(Envelope, LuaLibraryName, InstanceId));
        }

        public void Stop()
        {
            EventBus.PublishEvent(InternalEventFactory.CreateInternalInstanceRemoved(Envelope, LuaLibraryName, InstanceId));
        }
    }
}