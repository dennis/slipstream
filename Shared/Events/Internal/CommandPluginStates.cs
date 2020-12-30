#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginStates : IEvent
    {
        public string EventType => "CommandPluginStates";
        public bool ExcludeFromTxrx => true;
    }
}
