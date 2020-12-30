#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileChanged : IEvent
    {
        public string EventType => "FileMonitorFileChanged";
        public bool ExcludeFromTxrx => true;
        public string? FilePath { get; set; }
    }
}
