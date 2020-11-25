namespace Slipstream.Shared.Events.Setting
{
    public class TwitchSettings : IEvent
    {
        public string EventType => "TwitchSettings";

        public string TwitchUsername { get; set; } = string.Empty;
        public string TwitchToken { get; set; } = string.Empty;
    }
}
