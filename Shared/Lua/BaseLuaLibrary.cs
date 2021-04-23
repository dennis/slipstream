#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Diagnostics;

namespace Slipstream.Shared.Lua
{
    // As opposed to SingletonLuaLibrary, we got CommidityLuaLibrary. This creates instances, one for each unique index

    public abstract class BaseLuaLibrary<TInstance, TReference> : ILuaLibrary
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        protected readonly object Lock = new object();
        protected readonly DictionaryValidator Validator;
        protected readonly ILifetimeScope LifetimeScope;

        // LuaLibrary from class name and use that
        public string Name => $"api/{GetType().Name.Replace("LuaLibrary", "").ToLower()}";

        public BaseLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope)
        {
            Validator = validator;
            LifetimeScope = lifetimeScope;
        }

        public abstract void Dispose();

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            Validator.Validate(cfg);

            var instanceId = cfg.Get<string>("id");

            lock (Lock)
            {
                Debug.WriteLine($"Creating instance for '{Name}' with id '{instanceId}'");

                HandleInstance(instanceId, cfg);

                return LifetimeScope.Resolve<TReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }

        protected abstract TInstance CreateInstance(ILifetimeScope scope, Parameters cfg);

        protected abstract void HandleInstance(string instanceId, Parameters cfg);
    }
}