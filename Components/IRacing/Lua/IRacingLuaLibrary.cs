#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingLuaLibrary : ILuaLibrary
    {
        public string Name => "api/iracing";

        public static DictionaryValidator ConfigurationValidator { get; }

        private readonly object Lock = new object();
        private readonly ILifetimeScope LifetimeScope;
        private IIRacingInstanceThread? Instance;

        static IRacingLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitBool("send_raw_state");
        }

        public IRacingLuaLibrary(ILifetimeScope scope)
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
            var publishRawState = cfg.ExtractOrDefault("send_raw_state", false);

            lock (Lock)
            {
                if (Instance == null)
                {
                    Instance = LifetimeScope.Resolve<IIRacingInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("publishRawState", publishRawState)
                    );

                    Instance.Start();
                }
            }

            return LifetimeScope.Resolve<IIRacingReference>(
                new NamedParameter("instanceId", instanceId)
            );
        }
    }
}