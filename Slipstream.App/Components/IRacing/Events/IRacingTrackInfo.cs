#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTrackInfo : IEvent
    {
        public string EventType => nameof(IRacingTrackInfo);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public long TrackId { get; set; }
        public string TrackLength { get; set; } = string.Empty;
        public string TrackDisplayName { get; set; } = string.Empty;
        public string TrackCity { get; set; } = string.Empty;
        public string TrackCountry { get; set; } = string.Empty;
        public string TrackDisplayShortName { get; set; } = string.Empty;
        public string TrackConfigName { get; set; } = string.Empty;
        public string TrackType { get; internal set; } = string.Empty;
    }
}