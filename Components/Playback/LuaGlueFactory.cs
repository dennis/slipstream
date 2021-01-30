using Slipstream.Shared;

namespace Slipstream.Components.Playback
{
    internal partial class Playback
    {
        internal class LuaGlueFactory : ILuaGlueFactory
        {
            private readonly IEventBus EventBus;
            private readonly IPlaybackEventFactory EventFactory;

            public LuaGlueFactory(IEventBus eventBus, IPlaybackEventFactory eventFactory)
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
}