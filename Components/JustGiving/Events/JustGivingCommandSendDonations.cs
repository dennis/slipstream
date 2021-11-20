using Slipstream.Shared;

namespace Slipstream.Components.JustGiving.Events
{
    public class JustGivingCommandSendDonations : IEvent
    {
        public string EventType => nameof(JustGivingCommandSendDonations);
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
    }
}