﻿#nullable enable

using Autofac;
using NLua;
using Slipstream.Components.Internal;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
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

        private class InstanceContainer<T>
        {
            public T Instance { get; set; }
            public int RefCount = 0;

            public InstanceContainer(T instance)
            {
                Instance = instance;
            }
        }

        private readonly Dictionary<string, InstanceContainer<TInstance>> Instances = new Dictionary<string, InstanceContainer<TInstance>>();

        // LuaLibrary from class name and use that
        public string Name => $"api/{GetType().Name.Replace("LuaLibrary", "").ToLower()}";

        protected BaseLuaLibrary(DictionaryValidator validator, ILifetimeScope lifetimeScope, IEventBus eventBus)
        {
            Validator = validator;
            LifetimeScope = lifetimeScope;
            EventBus = eventBus;
        }

        public void Dispose()
        {
            foreach (var thread in Instances)
            {
                Debug.WriteLine($"[{thread.Key}] Disposing");
                thread.Value.Instance.Dispose();
            }
            Instances.Clear();
        }

        public ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            Validator.Validate(cfg);

            var instanceId = cfg.Get<string>("id");

            lock (Lock)
            {
                HandleInstance(instanceId, cfg);

                Debug.Assert(Instances.ContainsKey(instanceId));
                var container = Instances[instanceId];

                container.RefCount++;

                return LifetimeScope.Resolve<TReference>(
                    new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                    new NamedParameter("instanceId", instanceId),
                    new NamedParameter("luaLibrary", this)
                );
            }
        }

        protected abstract TInstance CreateInstance(ILifetimeScope scope, Parameters cfg);

        protected void HandleInstance(string instanceId, Parameters cfg)
        {
            if (!Instances.ContainsKey(instanceId))
            {
                Debug.WriteLine($"[{instanceId}] Creating instance {GetType().Name}");

                var instance = CreateInstance(LifetimeScope, cfg);
                Instances.Add(instanceId, new InstanceContainer<TInstance>(instance));
                instance.Start();
            }
        }
    }
}