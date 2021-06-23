using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchUserSubscribed : IEvent
    {
        public string EventType => nameof(TwitchUserSubscribed);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string SystemMessage { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = string.Empty;
        public long CumulativeMonths { get; set; }
        public long StreakMonths { get; set; }
    }
}