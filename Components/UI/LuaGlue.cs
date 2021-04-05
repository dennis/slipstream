using Serilog;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.UI
{
    public class LuaGlue : ILuaGlue
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IUIEventFactory EventFactory;
        private readonly string Prefix;

        public LuaGlue(ILogger logger, IEventBus eventBus, IUIEventFactory eventFactory, string prefix)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;
            Prefix = prefix;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["ui"] = this;
            lua.DoString(@"
function print(s); ui:print(s); end
function create_button(a); ui:create_button(a); end
function delete_button(a); ui:delete_button(a); end
");
        }

        public void Loop()
        {
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
    }
}