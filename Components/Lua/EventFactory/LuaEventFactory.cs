using Slipstream.Components.Internal;
using Slipstream.Components.Lua.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Lua.EventFactory
{
    public class LuaEventFactory : ILuaEventFactory
    {
        private readonly IEventSerdeService EventSerdeService;

        public LuaEventFactory(IEventSerdeService eventSerdeService)
        {
            EventSerdeService = eventSerdeService;
        }

        public LuaCommandDeduplicateEvents CreateLuaCommandDeduplicateEvents(IEventEnvelope envelope, IEvent[] events)
        {
            string json = "";

            foreach (var e in events)
            {
                json += EventSerdeService.Serialize(e) + "\n";
            }

            return new LuaCommandDeduplicateEvents
            {
                Envelope = envelope,
                Events = json
            };
        }
    }
}