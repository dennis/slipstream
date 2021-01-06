#nullable enable

using System.Collections.Generic;

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

        public override bool Equals(object? obj)
        {
            return obj is IRacingTrackInfo info &&
                   EventType == info.EventType &&
                   ExcludeFromTxrx == info.ExcludeFromTxrx &&
                   TrackId == info.TrackId &&
                   TrackLength == info.TrackLength &&
                   TrackDisplayName == info.TrackDisplayName &&
                   TrackCity == info.TrackCity &&
                   TrackCountry == info.TrackCountry &&
                   TrackDisplayShortName == info.TrackDisplayShortName &&
                   TrackConfigName == info.TrackConfigName &&
                   TrackType == info.TrackType;
        }

        public override int GetHashCode()
        {
            int hashCode = 1140221005;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + TrackId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackLength);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackDisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackCity);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackCountry);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackDisplayShortName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackConfigName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TrackType);
            return hashCode;
        }
    }
}