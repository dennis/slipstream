using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Lua
{
    public class TwitchLuaReference : ITwitchLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly ITwitchEventFactory EventFactory;
        private readonly TwitchLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public TwitchLuaReference(TwitchLuaLibrary luaLibrary, string instanceId, IEventBus eventBus, ITwitchEventFactory eventFactory)
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
        public void send_channel_message(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendMessage(InstanceId, message));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_whisper_message(string to, string message)
        {
            EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendWhisper(InstanceId, to, message));
        }
    }
}