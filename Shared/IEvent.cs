using System;

namespace Slipstream.Shared
{
    public interface IEvent
    {
        string EventType { get; }
        UInt64 Uptime { get; set; }
    }
}