#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class UtilLuaLibrary : BaseLuaLibrary<NoopInstanceThread, UtilLuaReference>, ILuaLibraryAutoRegistration
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static UtilLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator().RequireString("id");
        }

        public UtilLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override NoopInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");

            var subscription = EventBus.RegisterListener(instanceId);

            return LifetimeScope.Resolve<NoopInstanceThread>(
                new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("luaLibrary", this),
                new TypedParameter(typeof(IEventBusSubscription), subscription)
            );
        }
    }
}