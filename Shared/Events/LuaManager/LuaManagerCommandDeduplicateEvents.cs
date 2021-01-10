using System.Collections.Generic;

namespace Slipstream.Shared.Events.LuaManager
{
    public class LuaManagerCommandDeduplicateEvents : IEvent
    {
        public string EventType => "LuaManagerCommandDeduplicateEvents";

        public bool ExcludeFromTxrx => true;

        public string Events { get; set; } = "";

        public override bool Equals(object obj)
        {
            return obj is LuaManagerCommandDeduplicateEvents events &&
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
