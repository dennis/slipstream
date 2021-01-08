#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSay : IEvent
    {
        public string EventType => "AudioCommandSay";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }
        public float? Volume { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AudioCommandSay say &&
                   EventType == say.EventType &&
                   ExcludeFromTxrx == say.ExcludeFromTxrx &&
                   Message == say.Message &&
                   Volume == say.Volume;
        }

        public override int GetHashCode()
        {
            int hashCode = 1098881401;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + Volume.GetHashCode();
            return hashCode;
        }
    }
}
