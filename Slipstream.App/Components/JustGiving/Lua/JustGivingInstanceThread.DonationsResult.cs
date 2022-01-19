#nullable enable

using System.Collections.Generic;

namespace Slipstream.Components.JustGiving.Lua
{
    public partial class JustGivingInstanceThread
    {
        public class DonationsResult
        {
            public class PaginationInfo
            {
                public int PageNumber { get; set; }
                public int PageSizeRequested { get; set; }
                public int PageSizeReturned { get; set; }
                public int TotalPages { get; set; }
                public int TotalResults { get; set; }
            }

            public class Donation
            {
                public long Id { get; set; }
                public string Amount { get; set; } = string.Empty;
                public string CurrencyCode { get; set; } = string.Empty;
                public string DonationDate { get; set; } = string.Empty;
                public string DonorDisplayName { get; set; } = string.Empty;
                public string DonorLocalAmount { get; set; } = string.Empty;
                public string DonorLocalCurrencyCode { get; set; } = string.Empty;
                public string Message { get; set; } = string.Empty;
            }

            public string Id { get; set; } = string.Empty;
            public string PageShortName { get; set; } = string.Empty;
            public PaginationInfo Pagination { get; set; } = new PaginationInfo();
            public List<Donation> Donations { get; set; } = new List<Donation>();
        }
    }
}