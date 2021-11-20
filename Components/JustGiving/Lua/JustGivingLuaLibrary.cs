#nullable enable

using Autofac;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;

using System;

namespace Slipstream.Components.JustGiving.Lua
{
    public class JustGivingLuaLibrary : BaseLuaLibrary<IJustGivingInstanceThread, IJustGivingLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static JustGivingLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("appid")
                .RequireString("page");
        }

        public JustGivingLuaLibrary(ILifetimeScope scope, IEventBus eventBus) : base(ConfigurationValidator, scope, eventBus)
        {
        }

        protected override IJustGivingInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            string instanceId = cfg.Extract<String>("id");
            string appId = cfg.Extract<String>("appid");
            string pageShortName = cfg.Extract<String>("page");

            var subscription = EventBus.RegisterListener(instanceId);

            return scope.Resolve<IJustGivingInstanceThread>(
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new TypedParameter(typeof(IEventBusSubscription), subscription),
                new NamedParameter("appId", appId),
                new NamedParameter("pageShortName", pageShortName)
            );
        }
    }
}