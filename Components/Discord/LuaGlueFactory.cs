using Slipstream.Shared;

namespace Slipstream.Components.Discord
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory EventFactory;

        public LuaGlueFactory(IEventBus eventBus, IDiscordEventFactory eventFactory)
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