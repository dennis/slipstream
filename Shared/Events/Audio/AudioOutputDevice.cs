#nullable enable


namespace Slipstream.Shared.Events.Audio
{
    public class AudioOutputDevice : IEvent
    {
        public string EventType => "AudioOutputDevice";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVAILD-PLUGIN-ID";
        public string Product { get; set; } = string.Empty;
        public int DeviceIdx { get; set; } = -1;
    }
}
