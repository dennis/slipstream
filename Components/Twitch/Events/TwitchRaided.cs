using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchRaided : IEvent
    {
        public string EventType => "TwitchRaided";
        public ulong Uptime { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ViewerCount { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TwitchRaided raided &&
                   EventType == raided.EventType &&
                   Name == raided.Name &&
                   ViewerCount == raided.ViewerCount;
        }

        public override int GetHashCode()
        {
            int hashCode = -1039997614;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + ViewerCount.GetHashCode();
            return hashCode;
        }
    }
}