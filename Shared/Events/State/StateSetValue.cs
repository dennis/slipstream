#nullable enable

namespace Slipstream.Shared.Events.State
{
    public class StateSetValue : IEvent
    {
        public string EventType => "StateSetValue";
        public string Key { get; set; } = "";
        public string? Value { get; set; }
    }
}
