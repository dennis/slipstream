using NLua;
using Slipstream.Components.Internal;
using Slipstream.Shared;

namespace Slipstream.Components.Lua.Lua
{
    public class UtilLuaLibrary : ILuaLibrary
    {
        private readonly IEventSerdeService EventSerdeService;

        public string Name => "api/util";

        public UtilLuaLibrary(IEventSerdeService eventSerdeService)
        {
            EventSerdeService = eventSerdeService;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfg)
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public string event_to_json(IEvent @event)
        {
            return EventSerdeService.Serialize(@event);
        }
    }
}