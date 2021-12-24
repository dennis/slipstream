#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.WebServer.Lua
{
    public class WebServerLuaLibrary : BaseLuaLibrary<IWebServerInstanceThread, WebServerLuaReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static WebServerLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireLong("port");
        }

        public WebServerLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IWebServerInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var port = cfg.Extract<long>("port");
            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IWebServerInstanceThread>(
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