using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIReference : BaseLuaReference, IWinFormUIReference
    {
        private readonly IEventBus EventBus;
        private readonly IWinFormUIEventFactory EventFactory;
        private readonly ILogger Logger;

        public WinFormUIReference(
            string instanceId,
            string luaScriptInstanceId,
            IEventBus eventBus,
            IWinFormUIEventFactory eventFactory,
            ILogger logger) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            Logger = logger;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void print(string s)
        {
            Logger.Information(s);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void create_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateWinFormUICommandCreateButton(Envelope, text));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void delete_button(string text)
        {
            EventBus.PublishEvent(EventFactory.CreateWinFormUICommandDeleteButton(Envelope, text));
        }
    }
}