#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;
using System.Collections.Generic;

namespace Slipstream.Components.Lua.Lua
{
    public class LuaLuaLibrary : ILuaLuaLibrary
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        public string Name => "api/lua";
        private readonly object Lock = new object();
        private readonly Dictionary<string, ILuaInstanceThread> Instances = new Dictionary<string, ILuaInstanceThread>();
        private readonly ILifetimeScope LifetimeScope;
        private readonly IEventBus EventBus;

        static LuaLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("file");
        }

        public LuaLuaLibrary(ILifetimeScope scope, IEventBus eventBus)
        {
            LifetimeScope = scope;
            EventBus = eventBus;
        }

        internal void ReferenceDrop(LuaLuaReference _)
        {
        }

        public void Dispose()
        {
        }

        public ILuaReference? GetInstance(string _, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var filePath = cfg.Extract<string>("file");

            return LoadLuaFile(instanceId, instanceId, filePath);
        }

        public ILuaReference LoadLuaFile(string luaScriptInstanceId, string instanceId, string filePath)
        {
            lock (Lock)
            {
                if (!Instances.TryGetValue(instanceId, out ILuaInstanceThread serviceThread))
                {
                    var subscription = EventBus.RegisterListener(instanceId);
                    var newServiceThread = LifetimeScope.Resolve<ILuaInstanceThread>(
                        new NamedParameter("luaLibrary", this),
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("filePath", filePath),
                        new TypedParameter(typeof(IEventBusSubscription), subscription)
                    );

                    Instances.Add(instanceId, newServiceThread);
                }
            }

            return LifetimeScope.Resolve<ILuaLuaReference>(
                new NamedParameter("luaLibrary", this),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                new NamedParameter("serviceThread", Instances[instanceId])
            );
        }

        public void InstanceStopped(string instanceId)
        {
            lock (Lock)
            {
                if (Instances.ContainsKey(instanceId))
                {
                    Instances.Remove(instanceId);
                }
            }
        }
    }
}