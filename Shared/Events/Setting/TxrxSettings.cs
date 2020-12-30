#nullable enable

namespace Slipstream.Shared.Events.Setting
{
    public class TxrxSettings : IEvent
    {
        public string EventType => "TxrxSettings";
        public bool ExcludeFromTxrx => true;
        public string TxrxIpPort { get; set; } = "";
    }
}
