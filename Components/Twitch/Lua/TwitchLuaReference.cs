using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Twitch.Lua
{
    public class TwitchLuaReference : BaseLuaReference, ITwitchLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly ITwitchEventFactory EventFactory;
        private readonly TwitchLuaLibrary LuaLibrary;

        public TwitchLuaReference(
            TwitchLuaLibrary luaLibrary,
            string instanceId,
            string luaScriptInstanceId,
            IEventBus eventBus,
            ITwitchEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
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
        public void send_channel_message(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendMessage(Envelope, message));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_whisper_message(string to, string message)
        {
            EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendWhisper(Envelope, to, message));
        }
    }
}