using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class PlaybackMethodCollection
    {
        private readonly IEventBus EventBus;
        private readonly IPlaybackEventFactory EventFactory;

        public static PlaybackMethodCollection Register(IEventBus eventBus, IPlaybackEventFactory eventFactory, Lua lua)
        {
            var m = new PlaybackMethodCollection(eventBus, eventFactory);
            m.Register(lua);
            return m;
        }

        public PlaybackMethodCollection(IEventBus eventBus, IPlaybackEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Register(Lua lua)
        {
            lua["playback"] = this;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void save(string filename)
        {
            EventBus.PublishEvent(EventFactory.CreatePlaybackCommandSaveEvents(filename));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void load(string filename)
        {
            EventBus.PublishEvent(EventFactory.CreatePlaybackCommandInjectEvents(filename));
        }
    }
}