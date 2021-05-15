using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordCommandSendMessage : IEvent
    {
        public string EventType => "DiscordCommandSendMessage";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public ulong ChannelId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool TextToSpeech { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DiscordCommandSendMessage message &&
                   EventType == message.EventType &&
                   InstanceId == message.InstanceId &&
                   ChannelId == message.ChannelId &&
                   Message == message.Message &&
                   TextToSpeech == message.TextToSpeech;
        }

        public override int GetHashCode()
        {
            int hashCode = 514672855;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + ChannelId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + TextToSpeech.GetHashCode();
            return hashCode;
        }
    }
}