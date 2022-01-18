#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaLibrary : BaseLuaLibrary<IDiscordInstanceThread, DiscordLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static DiscordLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("token");
        }

        public DiscordLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IDiscordInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var token = cfg.Extract<string>("token");

            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IDiscordInstanceThread>(
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("token", token),
                new TypedParameter(typeof(IEventBusSubscription), subscription)
            );
        }
    }
}