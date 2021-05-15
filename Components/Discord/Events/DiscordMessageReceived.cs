using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordMessageReceived : IEvent
    {
        public string EventType => "DiscordMessageReceived";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public ulong FromId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public ulong ChannelId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DiscordMessageReceived received &&
                   EventType == received.EventType &&
                   InstanceId == received.InstanceId &&
                   From == received.From &&
                   FromId == received.FromId &&
                   Message == received.Message &&
                   Channel == received.Channel &&
                   ChannelId == received.ChannelId;
        }

        public override int GetHashCode()
        {
            int hashCode = -1000146541;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + FromId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Channel);
            hashCode = hashCode * -1521134295 + ChannelId.GetHashCode();
            return hashCode;
        }
    }
}