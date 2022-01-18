#nullable enable

using Autofac;

using NLua;

using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Shared.Lua
{
    public abstract class BaseLuaLibrary<TInstance, TReference> : ILuaLibraryAutoRegistration
        where TInstance : ILuaInstanceThread
        where TReference : ILuaReference
    {
        protected readonly object Lock = new object();
        protected readonly DictionaryValidator Validator;
        protected readonly ILifetimeScope LifetimeScope;
        protected readonly IEventBus EventBus;
        private readonly IInstanceIdTypeTracker InstanceIdTypeTracker;
        private readonly Dictionary<string, TInstance> Instances = new Dictionary<string, TInstance>();

        // LuaLibrary from class name and use that
        public string Name => $"api/{GetType().Name.Replace("LuaLibrary", "").ToLower()}";

        protected BaseLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope, IEventBus eventBus, IInstanceIdTypeTracker instanceIdTypeTracker)
        {
            Validator = validator;
            LifetimeScope = lifetimeScope;
            EventBus = eventBus;
            InstanceIdTypeTracker = instanceIdTypeTracker;
        }

        public void Dispose()
        {
            foreach (var thread in Instances)
            {
                Debug.WriteLine($"[{thread.Key}] Disposing");
                thread.Value.Dispose();
            }
            Instances.Clear();

            GC.SuppressFinalize(this);
        }

        public ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            Validator.Validate(cfg);

            var instanceId = cfg.Get<string>("id");

            InstanceIdTypeTracker.Verify(instanceId, Name);

            lock (Lock)
            {
                HandleInstance(luaScriptInstanceId, instanceId, cfg);

                Debug.Assert(Instances.ContainsKey(instanceId));
                var container = Instances[instanceId];

                return LifetimeScope.Resolve<TReference>(
                    new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                    new NamedParameter("instanceId", instanceId),
                    new NamedParameter("luaLibrary", this)
                );
            }
        }

        protected abstract TInstance CreateInstance(ILifetimeScope scope, string luaScriptInstanceId, Parameters cfg);

        protected virtual void HandleInstance(string luaScriptInstanceId, string instanceId, Parameters cfg)
        {
            // Remove instance if it is stopped meanwhile
            if (Instances.TryGetValue(instanceId, out TInstance instance))
            {
                if (instance.Stopped)
                {
                    Instances.Remove(instanceId);
                    instance.Dispose();
                }
            }

            if (!Instances.ContainsKey(instanceId))
            {
                Debug.WriteLine($"[{instanceId}] Creating instance {GetType().Name}");

                var newInstance = CreateInstance(LifetimeScope, luaScriptInstanceId, cfg);
                Instances.Add(instanceId, newInstance);
                newInstance.Start();
            }
        }

        protected void RemoveInstance(string instanceId)
        {
            Instances.Remove(instanceId);
        }
    }
}