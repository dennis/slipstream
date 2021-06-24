#nullable enable

using Autofac;
using Slipstream.Components.Internal;
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

        public DiscordLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(ConfigurationValidator, scope, eventBus, eventFactory)
        {
        }

        protected override IDiscordInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var token = cfg.Extract<string>("token");

            return scope.Resolve<IDiscordInstanceThread>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("token", token)
            );
        }
    }
}