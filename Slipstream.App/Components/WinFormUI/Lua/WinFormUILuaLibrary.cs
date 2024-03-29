﻿#nullable enable

using Autofac;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUILuaLibrary : BaseLuaLibrary<IWinFormUIInstanceThread, IWinFormUIReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static WinFormUILuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .PermitBool("deepview");
        }

        public WinFormUILuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker) : base(ConfigurationValidator, scope, eventBus, instanceIdTypeTracker)
        {
        }

        protected override IWinFormUIInstanceThread CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");
            var deepview = cfg.ExtractOrDefault("deepview", false);

            return scope.Resolve<IWinFormUIInstanceThread>(
                new NamedParameter("luaLibraryName", Name),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("deepView", deepview)
            );
        }
    }
}