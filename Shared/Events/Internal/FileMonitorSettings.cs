#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorSettings : IEvent
    {
        public string[]? Paths { get; set; }
    }
}
