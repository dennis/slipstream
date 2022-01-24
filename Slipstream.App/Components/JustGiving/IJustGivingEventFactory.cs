#nullable enable

using Slipstream.Components.JustGiving.Events;
using Slipstream.Shared;

namespace Slipstream.Components.JustGiving
{
    public interface IJustGivingEventFactory
    {
        JustGivingDonation CreateJustGivingDonation(IEventEnvelope envelope, string pageShortName, decimal amount, string currencyCode, long donationId, string donorDisplayName, decimal localAmount, string donorLocalCurrencyCode, string message);

        JustGivingCommandSendDonations CreateJustGivingCommandSendDonations(IEventEnvelope envelope);

        JustGivingInfo CreateJustGivingInfo(IEventEnvelope envelope, string pageShortName, string name, string currencySymbol, string currencyCode, string title, decimal fundrasisingTarget, decimal FundraisingGrandTotal, string summary);
    }
}