using Slipstream.Backend.Services;
using Slipstream.Shared.Events.LuaManager;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class LuaEventFactory : ILuaEventFactory
    {
        private readonly IEventSerdeService EventSerdeService;
        public LuaEventFactory(IEventSerdeService eventSerdeService)
        {
            EventSerdeService = eventSerdeService;
        }

        public LuaManagerCommandDeduplicateEvents CreateLuaManagerCommandDeduplicateEvents(IEvent[] events)
        {
            string json = "";

            foreach (var e in events)
            {
                json += EventSerdeService.Serialize(e) + "\n";
            }

            return new LuaManagerCommandDeduplicateEvents
            {
                Events = json
            };
        }
    }
}
