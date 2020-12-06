#nullable enable


namespace Slipstream.Shared.Events.Internal
{
    public class PluginRegister : IEvent
    {
        public string EventType => "PluginRegister";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
    }
}
