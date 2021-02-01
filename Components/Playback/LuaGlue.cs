using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Playback
{
    public class LuaGlue : ILuaGlue
    {
        private readonly IEventBus EventBus;
        private readonly IPlaybackEventFactory EventFactory;

        public LuaGlue(IEventBus eventBus, IPlaybackEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["playback"] = this;
        }

        public void Loop()
        {
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