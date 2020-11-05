#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginLoad : IEvent
    {
        public string? PluginName { get; set; }
        public bool? Enabled { get; set; }
        public IEvent? Settings { get; set; }
    }
}
