using Slipstream.Shared;

namespace Slipstream.Components.Internal.LuaGlues
{
    internal class InternalLuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        public InternalLuaGlueFactory(IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            return new InternalLuaGlue(EventBus, EventFactory);
        }
    }
}