#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandPlay : IEvent
    {
        public string EventType => "AudioCommandPlay";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string PluginId { get; set; } = "INVAILD-PLUGIN-ID";
        public string Filename { get; set; } = string.Empty;
        public float Volume { get; set; } = 1.0f;

        public override bool Equals(object? obj)
        {
            return obj is AudioCommandPlay play &&
                   EventType == play.EventType &&
                   ExcludeFromTxrx == play.ExcludeFromTxrx &&
                   Filename == play.Filename &&
                   Volume == play.Volume;
        }

        public override int GetHashCode()
        {
            int hashCode = 2126878269;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = (hashCode * -1521134295) + ExcludeFromTxrx.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Filename);
            hashCode = (hashCode * -1521134295) + Volume.GetHashCode();
            return hashCode;
        }
    }
}
