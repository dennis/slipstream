using Slipstream.Shared;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaChannelReference : IDiscordLuaChannelReference
    {
        private readonly string InstanceId;
        private readonly ulong ChannelId;
        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory EventFactory;

        public DiscordLuaChannelReference(string instanceId, long channelId, IEventBus eventBus, IDiscordEventFactory eventFactory)
        {
            InstanceId = instanceId;
            ChannelId = (ulong)channelId;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void send_message(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordCommandSendMessage(InstanceId, ChannelId, message, false));
        }

        public void send_message_tts(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordCommandSendMessage(InstanceId, ChannelId, message, true));
        }
    }
}