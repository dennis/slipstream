using Slipstream.Shared;

namespace Slipstream.Components.Lua.Events
{
    public class LuaCommandDeduplicateEvents : IEvent
    {
        public string EventType => nameof(LuaCommandDeduplicateEvents);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Events { get; set; } = "";
    }
}