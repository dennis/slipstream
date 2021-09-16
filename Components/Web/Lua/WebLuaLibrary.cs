#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Web.Lua
{
    public class WebLuaLibrary : BaseLuaLibrary<IWebInstanceThread, WebLuaReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static WebLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireLong("port");
        }

        public WebLuaLibrary(ILifetimeScope scope, IEventBus eventBus) : base(ConfigurationValidator, scope, eventBus)
        {
        }

        protected override IWebInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var port = cfg.Extract<long>("port");
            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IWebInstanceThread>(
               new NamedParameter("instanceId", instanceId),
               new NamedParameter("luaLibraryName", Name),
               new NamedParameter("port", port),
               new TypedParameter(typeof(IEventBusSubscription), subscription)
           );
        }

        public void InstanceDropped(string instanceId)
        {
            RemoveInstance(instanceId);
        }
    }
}