#nullable enable

namespace Slipstream.Shared.Events.State
{
    public class StateValue : IEvent
    {
        public string EventType => "StateValue";
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
