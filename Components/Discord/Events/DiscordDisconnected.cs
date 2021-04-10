using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordDisconnected : IEvent
    {
        public string EventType => "DiscordDisconnected";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is DiscordDisconnected disconnected &&
                   EventType == disconnected.EventType &&
                   ExcludeFromTxrx == disconnected.ExcludeFromTxrx &&
                   InstanceId == disconnected.InstanceId;
        }

        public override int GetHashCode()
        {
            int hashCode = -966063915;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            return hashCode;
        }
    }
}