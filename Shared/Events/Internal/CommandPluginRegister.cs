#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginRegister : IEvent
    {
        public string EventType => "CommandPluginRegister";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public IEvent? Settings;
    }
}
