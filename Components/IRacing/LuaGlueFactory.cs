using Slipstream.Shared;

namespace Slipstream.Components.IRacing
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;

        public LuaGlueFactory(IEventBus eventBus, IIRacingEventFactory eventFactory)
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