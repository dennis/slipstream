using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

#nullable enable

namespace Slipstream.Components.Internal.LuaGlues
{
    public class InternalLuaGlue : ILuaGlue
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        private static readonly DictionaryValidator RegisterPluginValidator;

        static InternalLuaGlue()
        {
            RegisterPluginValidator = new DictionaryValidator()
                .PermitString("plugin_id")
                .RequireString("plugin_name")
                .AllowAnythingElse(); // open int op for configuration
        }

        public InternalLuaGlue(IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["internal"] = this;
            lua.DoString(@"
function register_plugin(args); internal:register_plugin(args); end
function unregister_plugin(id); internal:unregister_plugin(id); end
");
        }

        public void Loop()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void register_plugin(LuaTable @params)
        {
            var config = Parameters.From(@params);
            RegisterPluginValidator.Validate(config);

            var plugin_name = config.Extract<string>("plugin_name");
            var plugin_id = config.ExtractOrDefault("plugin_id", plugin_name);

            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginRegister(plugin_id, plugin_name, config));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void unregister_plugin(string id)
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister(id));
        }
    }
}