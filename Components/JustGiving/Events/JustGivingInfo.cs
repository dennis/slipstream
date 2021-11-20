using Slipstream.Shared;

namespace Slipstream.Components.JustGiving.Events
{
    public class JustGivingInfo : IEvent
    {
        public string EventType => nameof(JustGivingInfo);
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string PageShortName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal FundraisingGrandTotal { get; set; }
        public decimal FundraisingTarget { get; set; }
        public string Summary { get; set; } = string.Empty;
    }
}