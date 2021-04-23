#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Diagnostics;

namespace Slipstream.Shared.Lua
{
    public abstract class SingletonLuaLibrary<TInstance, TReference> : BaseLuaLibrary<TInstance, TReference>
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        private ILuaInstanceThread? Instance = null;

        public SingletonLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope) : base(validator, lifetimeScope)
        {
        }

        public override void Dispose()
        {
            Instance?.Dispose();
        }

        protected override void HandleInstance(string instanceId, Parameters cfg)
        {
            if (Instance == null)
            {
                Debug.WriteLine($"Creating {GetType().Name} for instanceId '{instanceId}' [singleton]");

                Instance = CreateInstance(LifetimeScope, cfg);
                Instance?.Start();
            }
        }
    }
}