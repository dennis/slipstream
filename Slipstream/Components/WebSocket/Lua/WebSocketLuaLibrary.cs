using Autofac;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared;
using System.Net;

namespace Slipstream.Components.WebSocket.Lua
{
    public class WebSocketLuaLibrary : BaseLuaLibrary<IWebSocketLuaInstanceThread, IWebSocketLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static WebSocketLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("endpoint");
        }

        public WebSocketLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IWebSocketLuaInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var endopint = cfg.Extract<string>("endpoint");

            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IWebSocketLuaInstanceThread>(
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("endpoint", endopint),
                new TypedParameter(typeof(IEventBusSubscription), subscription)
            );
        }
    }
}