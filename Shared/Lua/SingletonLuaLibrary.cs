#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

namespace Slipstream.Shared.Lua
{
    public abstract class SingletonLuaLibrary<TInstance, TReference> : BaseLuaLibrary<TInstance, TReference>
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        protected SingletonLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope) : base(validator, lifetimeScope)
        {
        }

        new protected void HandleInstance(string _, Parameters cfg)
        {
            // Force instanceId to be "singleton", so we never get more than one
            cfg["instanceId"] = "singleton";

            base.HandleInstance("singleton", cfg);
        }
    }
}