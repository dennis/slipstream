#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackLuaReference : IPlaybackLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IPlaybackEventFactory EventFactory;

        public PlaybackLuaReference(IEventBus eventBus, IPlaybackEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Dispose()
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