#nullable enable

using Autofac;
using Slipstream.Components.Internal;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Shared.Lua
{
    public abstract class SingletonLuaLibrary<TInstance, TReference> : BaseLuaLibrary<TInstance, TReference>
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        protected SingletonLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope, IEventBus eventBus, IInternalEventFactory eventFactory) : base(validator, lifetimeScope, eventBus, eventFactory)
        {
        }

        new protected void HandleInstance(string luaScriptInstanceId, string _, Parameters cfg)
        {
            // Force instanceId to be "singleton", so we never get more than one
            cfg["instanceId"] = "singleton";

            base.HandleInstance(luaScriptInstanceId, "singleton", cfg);
        }
    }
}