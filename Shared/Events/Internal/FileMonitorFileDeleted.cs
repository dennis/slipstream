#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileDeleted: IEvent
    {
        public string? FilePath { get; set; }
    }
}
