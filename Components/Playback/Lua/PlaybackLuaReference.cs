#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackLuaReference : BaseLuaReference, IPlaybackLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IPlaybackEventFactory EventFactory;
        private readonly PlaybackLuaLibrary LuaLibrary;

        public PlaybackLuaReference(string instanceId, string luaScriptInstanceId, PlaybackLuaLibrary luaLibrary, IEventBus eventBus, IPlaybackEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            LuaLibrary = luaLibrary;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public override void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void save(string filename)
        {
            EventBus.PublishEvent(EventFactory.CreatePlaybackCommandSaveEvents(Envelope, filename));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void load(string filename)
        {
            EventBus.PublishEvent(EventFactory.CreatePlaybackCommandInjectEvents(Envelope, filename));
        }
    }
}