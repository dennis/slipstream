namespace Slipstream.Shared.Events.Setting
{
    public class TwitchSettings : IEvent
    {
        public string EventType => "TwitchSettings";
        public bool ExcludeFromTxrx => true;
        public string TwitchUsername { get; set; } = string.Empty;
        public string TwitchChannel { get; set; } = string.Empty;
        public string TwitchToken { get; set; } = string.Empty;
        public bool TwitchLog { get; set; }
    }
}
