#nullable enable

namespace Slipstream.Shared.Events.Setting
{
    public class TxrxSettings : IEvent
    {
        public string EventType => "TxrxSettings";
        public string TxrxIpPort { get; set; } = "";
    }
}
