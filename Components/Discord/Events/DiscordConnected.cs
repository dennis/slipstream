using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordConnected : IEvent
    {
        public string EventType => "DiscordConnected";

        public bool ExcludeFromTxrx => false;

        public ulong Uptime { get; set; }
    }
}