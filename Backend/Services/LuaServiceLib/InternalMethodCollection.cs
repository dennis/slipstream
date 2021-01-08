using NLua;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class InternalMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;

            public static InternalMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, Lua lua)
            {
                var m = new InternalMethodCollection(eventBus, eventFactory);

                m.Register(lua);

                return m;
            }

            public InternalMethodCollection(IEventBus eventBus, IEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
            }

            public void Register(Lua lua)
            {
                lua["internal"] = this;
                lua.DoString(@"
function register_plugin(id,name); internal:register_plugin(id,name); end
function unregister_plugin(id); internal:unregister_plugin(id); end
");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void register_plugin(string id, string name)
            {
                EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginRegister(id, name));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void unregister_plugin(string id)
            {
                EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister(id));
            }
        }
    }
}
