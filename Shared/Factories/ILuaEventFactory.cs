using Slipstream.Shared.Events.LuaManager;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface ILuaEventFactory
    {
        LuaManagerCommandDeduplicateEvents CreateLuaManagerCommandDeduplicateEvents(IEvent[] events);
    }
}
