#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchReceivedCommand : IEvent
    {
        public string EventType => "TwitchReceivedCommand";
        public bool ExcludeFromTxrx => false;
        public string From { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Moderator { get; set; }
        public bool Subscriber { get; set; }
        public bool Vip { get; set; }
        public bool Broadcaster { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TwitchReceivedCommand command &&
                   EventType == command.EventType &&
                   ExcludeFromTxrx == command.ExcludeFromTxrx &&
                   From == command.From &&
                   Message == command.Message &&
                   Moderator == command.Moderator &&
                   Subscriber == command.Subscriber &&
                   Vip == command.Vip &&
                   Broadcaster == command.Broadcaster;
        }

        public override int GetHashCode()
        {
            int hashCode = -588097615;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
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
