using System;

namespace Slipstream.Shared
{
    public interface IEvent
    {
        string EventType { get; }
        IEventEnvelope Envelope { get; set; }
    }
}