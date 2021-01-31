using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordDisconnected : IEvent
    {
        public string EventType => "DiscordDisconnected";

        public bool ExcludeFromTxrx => false;

        public ulong Uptime { get; set; }
    }
}