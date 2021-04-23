#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaLibrary : CommodityLuaLibrary<IDiscordInstanceThread, DiscordLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static DiscordLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("token");
        }

        public DiscordLuaLibrary(ILifetimeScope scope) : base(ConfigurationValidator, scope)
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