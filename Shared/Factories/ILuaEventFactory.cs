using Slipstream.Shared.Events.Lua;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface ILuaEventFactory
    {
        LuaCommandDeduplicateEvents CreateLuaCommandDeduplicateEvents(IEvent[] events);
    }
}
