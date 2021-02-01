using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Playback.Events
{
    public class PlaybackCommandInjectEvents : IEvent
    {
        public string EventType => "PlaybackCommandInjectEvents";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string Filename { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is PlaybackCommandInjectEvents events &&
                   EventType == events.EventType &&
                   ExcludeFromTxrx == events.ExcludeFromTxrx &&
                   Filename == events.Filename;
        }

        public override int GetHashCode()
        {
            int hashCode = -1549740322;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = (hashCode * -1521134295) + ExcludeFromTxrx.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Filename);
            return hashCode;
        }
    }
}