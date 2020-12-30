#nullable enable

namespace Slipstream.Shared.Events.Setting
{
    public class AudioSettings : IEvent
    {
        public string EventType => "AudioSettings";
        public bool ExcludeFromTxrx => true;
        public string? Path { get; set; }
    }
}
