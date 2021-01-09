using NLua;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class UIMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;
            private readonly string Prefix;

            public static UIMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, string logPrefix, Lua lua)
            {
                var m = new UIMethodCollection(eventBus, eventFactory, logPrefix);

                m.Register(lua);

                return m;
            }

            public UIMethodCollection(IEventBus eventBus, IEventFactory eventFactory, string logPrefix)
            {
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
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"{Prefix}: {s}"));
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
}
