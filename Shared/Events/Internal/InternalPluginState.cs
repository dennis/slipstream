#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalPluginState : IEvent
    {
        public string EventType => "InternalPluginState";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public string DisplayName { get; set; } = "INVALID-DISPLAY-NAME";
        public string PluginStatus { get; set; } = "INVALID-STATE";
    }
}
