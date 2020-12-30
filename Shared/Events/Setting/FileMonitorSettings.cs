#nullable enable

namespace Slipstream.Shared.Events.Setting
{
    public class FileMonitorSettings : IEvent
    {
        public string EventType => "FileMonitorSettings";
        public bool ExcludeFromTxrx => true;
        public string[]? Paths { get; set; }
    }
}
