using Autofac;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Components.Twitch.Lua
{
    public class TwitchLuaLibrary : BaseLuaLibrary<ITwitchLuaInstanceThread, ITwitchLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static TwitchLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("token")
                .RequireString("username")
                .RequireString("channel")
                .PermitBool("log");
        }

        public TwitchLuaLibrary(ILifetimeScope scope) : base(ConfigurationValidator, scope)
        {
        }

        protected override ITwitchLuaInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var twitchToken = cfg.Extract<string>("token");
            var twitchUsername = cfg.Extract<string>("username");
            var twitchChannel = cfg.Extract<string>("channel");
            var twitchLog = cfg.ExtractOrDefault("log", false);

            return scope.Resolve<ITwitchLuaInstanceThread>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("twitchToken", twitchToken),
                new NamedParameter("twitchUsername", twitchUsername),
                new NamedParameter("twitchChannel", twitchChannel),
                new NamedParameter("twitchLog", twitchLog)
            );
        }
    }
}