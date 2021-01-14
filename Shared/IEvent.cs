using System;

namespace Slipstream.Shared
{
    public interface IEvent
    {
        string EventType { get; }
        bool ExcludeFromTxrx { get; }
        UInt64 Uptime { get; set; }
    }
}
