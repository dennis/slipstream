#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class Initialized : IEvent
    {
        public string EventType => "Initialized";
        public bool ExcludeFromTxrx => true;
    }
}
