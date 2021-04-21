#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateLuaLibrary : ILuaLibrary
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        private readonly object Lock = new object();
        private readonly Dictionary<string, IApplicationUpdateInstanceThread> Instances = new Dictionary<string, IApplicationUpdateInstanceThread>();
        private readonly ILifetimeScope LifetimeScope;

        public string Name => "api/applicationupdate";

        static ApplicationUpdateLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("location")
                .PermitBool("prerelease");
        }

        public ApplicationUpdateLuaLibrary(ILifetimeScope scope)
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
            var updateLocation = cfg.Extract<string>("location");
            var prerelease = cfg.ExtractOrDefault("prerelease", false);

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                if (!Instances.TryGetValue(instanceId, out IApplicationUpdateInstanceThread value))
                {
                    Debug.WriteLine($"Creating IAudioServiceThread for instanceId '{instanceId}'");

                    var service = LifetimeScope.Resolve<IApplicationUpdateInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("location", updateLocation),
                        new NamedParameter("prerelease", prerelease)
                    );

                    Instances.Add(instanceId, service);
                    service.Start();
                }

                return LifetimeScope.Resolve<IApplicationUpdateReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }
    }
}