using NLua;
using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class UIMethodCollection
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IUIEventFactory EventFactory;
        private readonly string Prefix;

        public static UIMethodCollection Register(ILogger logger, IEventBus eventBus, IUIEventFactory eventFactory, string logPrefix, Lua lua)
        {
            var m = new UIMethodCollection(logger, eventBus, eventFactory, logPrefix);

            m.Register(lua);

            return m;
        }

        public UIMethodCollection(ILogger logger, IEventBus eventBus, IUIEventFactory eventFactory, string logPrefix)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;
            Prefix = logPrefix;
        }

        public void Register(Lua lua)
        {
            lua["ui"] = this;
            lua.DoString(@"
function print(s); ui:print(s); end
function create_button(a); ui:create_button(a); end
function delete_button(a); ui:delete_button(a); end
");
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