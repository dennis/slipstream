#nullable enable

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Components.JustGiving.Events;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Slipstream.Components.JustGiving.Lua
{
    public partial class JustGivingInstanceThread : BaseInstanceThread, IJustGivingInstanceThread
    {
        private readonly IJustGivingEventFactory EventFactory;
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly string AppId;
        private readonly string PageShortName;
        private readonly Dictionary<long, DonationsResult.Donation> Donations = new Dictionary<long, DonationsResult.Donation>();
        private DateTime UpdatedAt { get; set; }
        private bool JustGivingInitialized { get; set; } = false;

        public JustGivingInstanceThread(
            string luaLibraryName,
            string instanceId,
            string appId,
            string pageShortName,
            IEventBus eventBus,
            IJustGivingEventFactory eventFactory,
            IEventBusSubscription eventBusSubscription,
            IEventHandlerController eventHandlerController,
            IInternalEventFactory internalEventFactory,
            ILogger logger) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            EventFactory = eventFactory;
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            AppId = appId;
            PageShortName = pageShortName;
        }

        protected override void Main()
        {
            var eventHandler = EventHandlerController.Get<JustGiving.EventHandler.JustGiving>();

            eventHandler.OnJustGivingCommandSendDonations += (_, e) => OnJustGivingCommandSendDonations(e);

            UpdateJustGivingData();

            while (!Stopping)
            {
                IEvent? @event;
                while ((@event = Subscription.NextEvent(100)) != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }

                // JustGiving says they cache for a minut, so make sure we delayed just a bit more
                if (UpdatedAt.AddMinutes(1.1) < DateTime.UtcNow)
                {
                    UpdateJustGivingData();
                }
            }
        }

        private void OnJustGivingCommandSendDonations(JustGivingCommandSendDonations e)
        {
            foreach (var donationId in Donations.Keys.OrderBy(a => a))
            {
                try
                {
                    EventBus.PublishEvent(ToDonationEvent(e.Envelope.Reply(InstanceId), Donations[donationId]));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"JustGiving [{PageShortName}]: Unexpected problem when creating JustGivingDonation event {ex.Message} {ex.StackTrace}");
                }
            }
        }

        private void UpdateJustGivingData()
        {
            var fetcher = new DonationFetcher(AppId, PageShortName, newDonation => NewDonation(newDonation), meta => NewMetaData(meta));
            var newDonations = fetcher.UpdateDonation(Donations);
            if (newDonations.Count > 0 || !JustGivingInitialized)
            {
                fetcher.UpdateMeta();
            }

            UpdatedAt = DateTime.UtcNow;
            JustGivingInitialized = true;
        }

        private void NewMetaData(DonationMeta meta)
        {
            try
            {
                EventBus.PublishEvent(EventFactory.CreateJustGivingInfo(
                    BroadcastEnvelope,
                    PageShortName,
                    meta.EventName,
                    meta.CurrencySymbol,
                    meta.CurrencyCode,
                    meta.Title,
                    decimal.Parse(meta.FundraisingTarget, NumberStyles.Currency, CultureInfo.InvariantCulture),
                    decimal.Parse(meta.GrandTotalRaisedExcludingGiftAid, NumberStyles.Currency, CultureInfo.InvariantCulture),
                    meta.PageSummary
                ));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"JustGiving [{PageShortName}]: Unexpected problem when creating JustGivingInfo event {e.Message} {e.StackTrace}");
            }
        }

        private void NewDonation(DonationsResult.Donation donation)
        {
            // Send event
            Debug.WriteLine($"JustGiving [{PageShortName}]: {donation.Id} {donation.DonorDisplayName} {donation.Message}");
            Donations.Add(donation.Id, donation);

            if (!JustGivingInitialized)
                return;

            /*
             * Sometimes we get these wierd empty donations. Let's ignore them
            {
                "amount": null,
                "currencyCode": "GBP",
                "donationDate": "/Date(1637416315000+0000)/",
                "donationRef": null,
                "donorDisplayName": "Anonymous",
                "id": 1080311874,
                "message": "",
                "source": "SponsorshipDonations",
                "thirdPartyReference": null,
                "charityId": 300
            },
            */

            if (donation.Amount == null)
            {
                Debug.WriteLine($"JustGiving [{PageShortName}]: {donation.Id} Ignored - not amount donated");
                return;
            }

            try
            {
                EventBus.PublishEvent(ToDonationEvent(InstanceEnvelope, donation));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"JustGiving [{PageShortName}]: Unexpected problem when creating JustGivingDonation event {e.Message} {e.StackTrace}");
            }
        }

        private IEvent ToDonationEvent(IEventEnvelope envelope, DonationsResult.Donation donation)
        {
            return EventFactory.CreateJustGivingDonation(
                envelope: envelope,
                pageShortName: PageShortName,
                amount: decimal.Parse(donation.Amount, NumberStyles.Currency, CultureInfo.InvariantCulture),
                currencyCode: donation.CurrencyCode,
                donationId: donation.Id,
                donorDisplayName: donation.DonorDisplayName,
                localAmount: decimal.Parse(donation.DonorLocalAmount, NumberStyles.Currency, CultureInfo.InvariantCulture),
                donorLocalCurrencyCode: donation.DonorLocalCurrencyCode,
                message: donation.Message
            );
        }

        public new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}