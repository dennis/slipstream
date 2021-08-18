#nullable enable

using Autofac;

using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Shared.Lua
{
    // This will at most create one instance with id "singleton". Can be used for eg. IRacing, where more than one instance
    // doesn't make sense.
    public abstract class SingletonLuaLibrary<TInstance, TReference> : BaseLuaLibrary<TInstance, TReference>
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        protected SingletonLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope, IEventBus eventBus) : base(validator, lifetimeScope, eventBus)
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