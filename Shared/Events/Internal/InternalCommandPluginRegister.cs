#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginRegister : IEvent
    {
        public string EventType => "InternalCommandPluginRegister";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public IEvent? Settings;
    }
}
