using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchGiftedSubscription : IEvent
    {
        public string EventType => nameof(TwitchGiftedSubscription);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Gifter { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string SystemMessage { get; set; } = string.Empty;
    }
}