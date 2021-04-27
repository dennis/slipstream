#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;
using System.Diagnostics;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateLuaLibrary : BaseLuaLibrary<IApplicationUpdateInstanceThread, IApplicationUpdateReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static ApplicationUpdateLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("location")
                .PermitBool("prerelease");
        }

        public ApplicationUpdateLuaLibrary(ILifetimeScope scope) : base(ConfigurationValidator, scope)
        {
        }

        protected override IApplicationUpdateInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var updateLocation = cfg.Extract<string>("location");
            var prerelease = cfg.ExtractOrDefault("prerelease", false);

            return scope.Resolve<IApplicationUpdateInstanceThread>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("location", updateLocation),
                new NamedParameter("prerelease", prerelease)
            );
        }
    }
}