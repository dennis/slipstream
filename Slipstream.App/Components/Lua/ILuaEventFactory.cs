using Slipstream.Components.Lua.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Lua
{
    public interface ILuaEventFactory
    {
        LuaCommandDeduplicateEvents CreateLuaCommandDeduplicateEvents(IEventEnvelope envelope, IEvent[] events);
    }
}