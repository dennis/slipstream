#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Components.Discord.Lua
{
    public class DiscordLuaLibrary : ILuaLibrary
    {
        private static readonly DictionaryValidator ConfigurationValidator;
        private readonly ILifetimeScope LifetimeScope;
        private readonly object Lock = new object();
        private readonly Dictionary<string, IDiscordInstanceThread> Instances = new Dictionary<string, IDiscordInstanceThread>();

        public string Name => "api/discord";

        static DiscordLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("token");
        }

        public DiscordLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var token = cfg.Extract<string>("token");

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                if (!Instances.TryGetValue(instanceId, out IDiscordInstanceThread value))
                {
                    Debug.WriteLine($"Creating {nameof(IDiscordInstanceThread)} for instanceId '{instanceId}'");

                    var service = LifetimeScope.Resolve<IDiscordInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("token", token)
                    );

                    Instances.Add(instanceId, service);
                    service.Start();
                }

                return LifetimeScope.Resolve<DiscordLuaReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }
    }
}