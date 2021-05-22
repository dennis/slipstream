#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;
using System.ComponentModel;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchReceivedMessage : IEvent
    {
        public string EventType => "TwitchReceivedMessage";
        public ulong Uptime { get; set; }
        
        [Description("InstanceId used for a Twitch connection")]
        public string InstanceId { get; set; } = string.Empty;

        [Description("User that sent the message")]
        public string From { get; set; } = string.Empty;

        [Description("Message body")]
        public string Message { get; set; } = string.Empty;

        [Description("True if the user is a moderator")]
        public bool Moderator { get; set; }

        [Description("True if the user is a subscriber")]
        public bool Subscriber { get; set; }

        [Description("True if the user is a VIP")]
        public bool Vip { get; set; }

        [Description("True if the user is the broadcaster")]
        public bool Broadcaster { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TwitchReceivedMessage message &&
                   EventType == message.EventType &&
                   InstanceId == message.InstanceId &&
                   From == message.From &&
                   Message == message.Message &&
                   Moderator == message.Moderator &&
                   Subscriber == message.Subscriber &&
                   Vip == message.Vip &&
                   Broadcaster == message.Broadcaster;
        }

        public override int GetHashCode()
        {
            int hashCode = -588097615;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + Moderator.GetHashCode();
            hashCode = hashCode * -1521134295 + Subscriber.GetHashCode();
            hashCode = hashCode * -1521134295 + Vip.GetHashCode();
            hashCode = hashCode * -1521134295 + Broadcaster.GetHashCode();
            return hashCode;
        }
    }
}