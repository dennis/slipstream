using System.Collections.Generic;

namespace Slipstream.Shared.Events.Lua
{
    public class LuaCommandDeduplicateEvents : IEvent
    {
        public string EventType => "LuaCommandDeduplicateEvents";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string Events { get; set; } = "";

        public override bool Equals(object obj)
        {
            return obj is LuaCommandDeduplicateEvents events &&
                   EventType == events.EventType &&
                   ExcludeFromTxrx == events.ExcludeFromTxrx &&
                   Events == events.Events;
        }

        public override int GetHashCode()
        {
            int hashCode = -1012215078;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Events);
            return hashCode;
        }
    }
}
