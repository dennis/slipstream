using Autofac;
using Autofac.Core.Lifetime;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Components.Twitch.Lua
{
    public class TwitchLuaLibrary : ILuaLibrary
    {
        private static DictionaryValidator ConfigurationValidator;
        private readonly ILifetimeScope LifetimeScope;
        private readonly object Lock = new object();
        private readonly Dictionary<string, ITwitchLuaInstanceThread> Instances = new Dictionary<string, ITwitchLuaInstanceThread>();

        public string Name => "api/twitch";

        static TwitchLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("token")
                .RequireString("username")
                .RequireString("channel")
                .PermitBool("log");
        }

        public TwitchLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var twitchToken = cfg.Extract<string>("token");
            var twitchUsername = cfg.Extract<string>("username");
            var twitchChannel = cfg.Extract<string>("channel");
            var twitchLog = cfg.ExtractOrDefault("log", false);

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                if (!Instances.TryGetValue(instanceId, out ITwitchLuaInstanceThread value))
                {
                    Debug.WriteLine($"Creating ITwitchLuaInstance for instanceId '{instanceId}'");

                    var service = LifetimeScope.Resolve<ITwitchLuaInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("twitchToken", twitchToken),
                        new NamedParameter("twitchUsername", twitchUsername),
                        new NamedParameter("twitchChannel", twitchChannel),
                        new NamedParameter("twitchLog", twitchLog)
                    );

                    Instances.Add(instanceId, service);
                    service.Start();
                }

                return LifetimeScope.Resolve<ITwitchLuaReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }
    }
}