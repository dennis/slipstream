using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.UI.Lua
{
    public class IUILibraryReference : ILuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IUIEventFactory EventFactory;
        private readonly ILogger Logger;
        private readonly string Prefix;
        private readonly UILuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public IUILibraryReference(UILuaLibrary luaLibrary, string instanceId, string prefix, IEventBus eventBus, IUIEventFactory eventFactory, ILogger logger)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
            Logger = logger;
            Prefix = prefix;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void print(string s)
        {
            Logger.Information($"{Prefix}: {s}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void create_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandCreateButton(text));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void delete_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandDeleteButton(text));
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}