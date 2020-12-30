#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingTrackInfo : IEvent
    {
        public string EventType => "IRacingTrackInfo";
        public bool ExcludeFromTxrx => false;
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