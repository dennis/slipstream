#nullable enable

using Autofac;

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

        public ApplicationUpdateLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IApplicationUpdateInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var updateLocation = cfg.Extract<string>("location");
            var prerelease = cfg.ExtractOrDefault("prerelease", false);

            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IApplicationUpdateInstanceThread>(
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new TypedParameter(typeof(IEventBusSubscription), subscription),
                new NamedParameter("location", updateLocation),
                new NamedParameter("prerelease", prerelease)
            );
        }
    }
}