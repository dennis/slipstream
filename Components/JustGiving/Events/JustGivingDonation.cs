using Slipstream.Shared;

namespace Slipstream.Components.JustGiving.Events
{
    public class JustGivingDonation : IEvent
    {
        public string EventType => nameof(JustGivingDonation);
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string PageShortName { get; set; } = string.Empty;
        public long DonationId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string DonorDisplayName { get; set; } = string.Empty;
        public decimal DonorLocalAmount { get; set; }
        public string DonorLocalCurrencyCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}