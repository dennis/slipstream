#nullable enable

using Autofac;

using Newtonsoft.Json;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

using System.Collections.Generic;

namespace Slipstream.Components.WebWidget.Lua
{
    public class WebWidgetLuaLibrary : BaseLuaLibrary<IWebWidgetInstanceThread, WebWidgetLuaReference>
    {
        private IHttpServer? HttpServer;
        public static DictionaryValidator ConfigurationValidator { get; }

        static WebWidgetLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitDictionary("data", a => a.AllowAnythingElse())
                .RequireString("type");
        }

        public WebWidgetLuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IWebWidgetInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var webWidgetType = cfg.Extract<string>("type");
            var data = cfg.ExtractOrDefault<Dictionary<dynamic, dynamic>>("data", null);
            var json = JsonConvert.SerializeObject(data);

            if (HttpServer == null)
            {
                var subscription = EventBus.RegisterListener(instanceId);
                HttpServer = LifetimeScope.Resolve<IHttpServer>(
                    new TypedParameter(typeof(IEventBusSubscription), subscription),
                    new TypedParameter(typeof(WebWidgetLuaLibrary), this)
                );
            }

            return scope.Resolve<IWebWidgetInstanceThread>(
               new NamedParameter("instanceId", instanceId),
               new NamedParameter("webWidgetType", webWidgetType),
               new NamedParameter("data", json),
               new NamedParameter("httpServer", HttpServer)
           );
        }

        public void InstanceDropped(string instanceId)
        {
            RemoveInstance(instanceId);
        }
    }
}