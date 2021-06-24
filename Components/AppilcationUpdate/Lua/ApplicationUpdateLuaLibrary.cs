#nullable enable

using Autofac;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateLuaLibrary : SingletonLuaLibrary<IApplicationUpdateInstanceThread, IApplicationUpdateReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static ApplicationUpdateLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("location")
                .PermitBool("prerelease");
        }

        public ApplicationUpdateLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(ConfigurationValidator, scope, eventBus, eventFactory)
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