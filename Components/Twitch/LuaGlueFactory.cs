using Slipstream.Shared;

namespace Slipstream.Components.Twitch
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly ITwitchEventFactory EventFactory;

        public LuaGlueFactory(IEventBus eventBus, ITwitchEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            return new LuaGlue(EventBus, EventFactory);
        }
    }
}