namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSetOutputDevice : IEvent
    {
        public string EventType => "AudioCommandSetOutputDevice";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVALID-PLUGIN-ID";
        public int DeviceIdx { get; set; } = -1;
    }
}