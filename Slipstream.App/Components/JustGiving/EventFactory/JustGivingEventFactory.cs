#nullable enable

using Slipstream.Components.JustGiving.Events;
using Slipstream.Shared;

namespace Slipstream.Components.JustGiving.EventFactory
{
    public class JustGivingEventFactory : IJustGivingEventFactory
    {
        public JustGivingDonation CreateJustGivingDonation(IEventEnvelope envelope, string pageShortName, decimal amount, string currencyCode, long donationId, string donorDisplayName, decimal localAmount, string donorLocalCurrencyCode, string message)
        {
            return new JustGivingDonation
            {
                Envelope = envelope,
                PageShortName = pageShortName,
                Amount = amount,
                CurrencyCode = currencyCode,
                DonationId = donationId,
                DonorDisplayName = donorDisplayName,
                DonorLocalAmount = localAmount,
                DonorLocalCurrencyCode = donorLocalCurrencyCode,
                Message = message
            };
        }

        public JustGivingCommandSendDonations CreateJustGivingCommandSendDonations(IEventEnvelope envelope)
        {
            return new JustGivingCommandSendDonations { Envelope = envelope };
        }

        public JustGivingInfo CreateJustGivingInfo(IEventEnvelope envelope, string pageShortName, string name, string currencySymbol, string currencyCode, string title, decimal fundrasisingTarget, decimal FundraisingGrandTotal, string summary)
        {
            return new JustGivingInfo
            {
                Envelope = envelope,
                PageShortName = pageShortName,
                Name = name,
                CurrencySymbol = currencySymbol,
                CurrencyCode = currencyCode,
                Title = title,
                FundraisingTarget = fundrasisingTarget,
                FundraisingGrandTotal = FundraisingGrandTotal,
                Summary = summary
            };
        }
    }
}