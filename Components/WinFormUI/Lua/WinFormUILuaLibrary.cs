#nullable enable

using Autofac;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared;
using Slipstream.Components.Internal;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUILuaLibrary : BaseLuaLibrary<IWinFormUIInstanceThread, IWinFormUIReference>
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        static WinFormUILuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id");
        }

        public WinFormUILuaLibrary(ILifetimeScope scope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(ConfigurationValidator, scope, eventBus, eventFactory)
        {
        }

        protected override IWinFormUIInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            var instanceId = cfg.Extract<string>("id");

            return scope.Resolve<IWinFormUIInstanceThread>(
                new NamedParameter("instanceId", instanceId)
            );
        }
    }
}