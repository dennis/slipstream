#nullable enable

using NLua;
using Slipstream.Components.Internal;
using Slipstream.Shared;

namespace Slipstream.Components.Lua.Lua
{
    public class InternalLuaLibrary : ILuaLibrary
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        public string Name => "api/internal";

        public InternalLuaLibrary(IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfg)
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void shutdown()
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandShutdown());
        }
    }
}