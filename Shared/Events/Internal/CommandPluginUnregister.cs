#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginUnregister : IEvent
    {
        public string EventType => "CommandPluginUnregister";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
