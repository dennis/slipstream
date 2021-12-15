#nullable enable

using Newtonsoft.Json;

using Serilog;

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Slipstream.Components.JustGiving.Lua
{
    public partial class JustGivingInstanceThread
    {
        public class DonationFetcher
        {
            private ILogger Logger { get; set; }
            private string AppId { get; }
            private string PageShortName { get; }
            private Action<DonationsResult.Donation> OnNewDonation { get; }
            private Action<DonationMeta> OnNewMeta { get; }
            private DonationMeta PreviousMeta = new DonationMeta();

            public DonationFetcher(ILogger logger, string appId, string pageShortName, Action<DonationsResult.Donation> onNewDonation, Action<DonationMeta> onNewMeta)
            {
                Logger = logger;
                AppId = appId;
                PageShortName = pageShortName;
                OnNewDonation = onNewDonation;
                OnNewMeta = onNewMeta;
            }

            public List<DonationsResult.Donation> UpdateDonation(IDictionary<long, DonationsResult.Donation> donations)
            {
                var firstDonationPageResult = FetchDonations(1);

                if (firstDonationPageResult == null)
                    return new List<DonationsResult.Donation>();

                var newDonations = new List<DonationsResult.Donation>();

                for (int pageNum = 1; pageNum <= firstDonationPageResult.Pagination.TotalPages; pageNum++)
                {
                    DonationsResult? donationsResult;

                    if (pageNum == 1)
                    {
                        // we already got it, so use reuse it
                        donationsResult = firstDonationPageResult;
                    }
                    else
                    {
                        donationsResult = FetchDonations(pageNum);
                        if (donationsResult == null)
                        {
                            // Something went wrong
                            return new List<DonationsResult.Donation>();
                        }
                    }

                    foreach (var item in donationsResult.Donations)
                    {
                        if (!donations.ContainsKey(item.Id))
                        {
                            newDonations.Add(item);
                        }
                        else
                        {
                            // We found a known donation, so let's not do any more pages
                            pageNum = firstDonationPageResult.Pagination.TotalPages + 1;
                        }
                    }
                }

                // If we're here, we got new donations
                newDonations.Reverse();
                newDonations.ForEach(OnNewDonation);

                return newDonations;
            }

            public void UpdateMeta()
            {
                HttpClient client = new HttpClient();
                var uri = $"http://api.justgiving.com/{AppId}/v1/fundraising/pages/{PageShortName}";

                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.TryAddWithoutValidation("Accept", "application/json");
                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    var currentMeta = JsonConvert.DeserializeObject<DonationMeta>(responseBody);

                    if (currentMeta != null && currentMeta.CompareTo(PreviousMeta) != 0)
                    {
                        OnNewMeta(currentMeta);
                        PreviousMeta = currentMeta;
                    }
                }
            }

            private DonationsResult? FetchDonations(int pageNum)
            {
                HttpClient client = new HttpClient();
                var uri = $"http://api.justgiving.com/{AppId}/v1/fundraising/pages/{PageShortName}/donations?pageNum={pageNum}&pageSize=100";

                Logger.Debug("JustGiving [{PageShortName}]: Fetch Donations page {PageNum}", PageShortName, pageNum);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.TryAddWithoutValidation("Accept", "application/json");
                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return JsonConvert.DeserializeObject<DonationsResult>(responseBody);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}