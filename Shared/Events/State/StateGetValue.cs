#nullable enable

namespace Slipstream.Shared.Events.State
{
    public class StateGetValue : IEvent
    {
        public string EventType => "StateGetValue";
        public string Key { get; set; } = "";
    }
}
