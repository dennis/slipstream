namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSendDevices : IEvent
    {
        public string EventType => "AudioCommandSendDevices";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVALID-PLUGIN-ID";
    }
}