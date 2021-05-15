#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackLuaReference : IPlaybackLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IPlaybackEventFactory EventFactory;
        private readonly PlaybackLuaLibrary LuaLibrary;

        public string InstanceId { get; }

        public PlaybackLuaReference(string instanceId, PlaybackLuaLibrary luaLibrary, IEventBus eventBus, IPlaybackEventFactory eventFactory)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
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