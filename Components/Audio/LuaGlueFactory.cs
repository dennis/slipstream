using Slipstream.Shared;

namespace Slipstream.Components.Audio
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly IAudioEventFactory EventFactory;

        public LuaGlueFactory(IEventBus eventBus, IAudioEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public ILuaGlue CreateLuaGlue()
        {
            return new LuaGlue(EventBus, EventFactory);
        }
    }
}