using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaChannelReference : BaseLuaReference, IDiscordLuaChannelReference
    {
        private readonly ulong ChannelId;
        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory EventFactory;

        public DiscordLuaChannelReference(string instanceId, string luaScriptInstanceId, long channelId, IEventBus eventBus, IDiscordEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            ChannelId = (ulong)channelId;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public override void Dispose()
        {
        }

        public void send_message(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordCommandSendMessage(Envelope, ChannelId, message, false));
        }

        public void send_message_tts(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordCommandSendMessage(Envelope, ChannelId, message, true));
        }
    }
}