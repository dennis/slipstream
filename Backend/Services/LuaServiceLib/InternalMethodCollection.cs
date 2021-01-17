using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class InternalMethodCollection
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        private static readonly ParameterValidator RegisterPlugin2Validator;

        static InternalMethodCollection()
        {
            RegisterPlugin2Validator = ParameterValidator.Create()
                .Permit("plugin_id", typeof(string))
                .Required("plugin_name", typeof(string))
                .AllowAnythingElse();
        }

        public static InternalMethodCollection Register(IEventBus eventBus, IInternalEventFactory eventFactory, Lua lua)
        {
            var m = new InternalMethodCollection(eventBus, eventFactory);

            m.Register(lua);

            return m;
        }

        public InternalMethodCollection(IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Register(Lua lua)
        {
            lua["internal"] = this;
            lua.DoString(@"
function register_plugin(args); internal:register_plugin(args); end
function unregister_plugin(id); internal:unregister_plugin(id); end
");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void register_plugin(LuaTable @params)
        {
            var validated = RegisterPlugin2Validator.Parse(@params);

            var plugin_name = validated.Extract<string>("plugin_name");
            var plugin_id = validated.ExtractOrDefault("plugin_id", plugin_name);
            var config = validated.AsDictionary();

            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginRegister(plugin_id, plugin_name, config));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void unregister_plugin(string id)
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister(id));
        }
    }
}