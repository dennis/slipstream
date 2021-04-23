#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Shared.Lua
{
    public abstract class CommodityLuaLibrary<TInstance, TReference> : BaseLuaLibrary<TInstance, TReference>
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        private readonly Dictionary<string, TInstance> Instances = new Dictionary<string, TInstance>();

        public CommodityLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope) : base(validator, lifetimeScope)
        {
        }

        public override void Dispose()
        {
            foreach (var thread in Instances)
            {
                thread.Value.Dispose();
            }
            Instances.Clear();
        }

        protected override void HandleInstance(string instanceId, Parameters cfg)
        {
            if (!Instances.ContainsKey(instanceId))
            {
                Debug.WriteLine($"Creating {GetType().Name} for instanceId '{instanceId}' [commodity]");

                var instance = CreateInstance(LifetimeScope, cfg);
                Instances.Add(instanceId, instance);
                instance.Start();
            }
        }
    }
}