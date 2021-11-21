#nullable enable

using System;

namespace Slipstream.Components.JustGiving.Lua
{
    public partial class JustGivingInstanceThread
    {
        public class DonationMeta : IComparable<DonationMeta>
        {
            public string EventName { get; set; } = string.Empty;
            public string CurrencySymbol { get; set; } = string.Empty;
            public string CurrencyCode { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string GrandTotalRaisedExcludingGiftAid { get; set; } = string.Empty;
            public string FundraisingTarget { get; set; } = string.Empty;
            public string PageSummary { get; set; } = string.Empty;

            public int CompareTo(DonationMeta? other)
            {
                if (other == null) return 1;

                if (EventName == other.EventName && CurrencySymbol == other.CurrencySymbol && CurrencyCode == other.CurrencyCode &&
                    Title == other.Title && GrandTotalRaisedExcludingGiftAid == other.GrandTotalRaisedExcludingGiftAid &&
                    FundraisingTarget == other.FundraisingTarget && PageSummary == other.PageSummary)
                    return 0;

                return -1;
            }
        }
    }
}