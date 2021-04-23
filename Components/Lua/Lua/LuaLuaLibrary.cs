#nullable enable

using Autofac;
using NLua;
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

        static LuaLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireString("file");
        }

        public LuaLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            var instanceId = cfg.Extract<string>("id");
            var filePath = cfg.Extract<string>("file");

            return instance(instanceId, filePath);
        }

        public ILuaReference instance(string instanceId, string filePath)
        {
            lock (Lock)
            {
                if (!Instances.TryGetValue(instanceId, out ILuaInstanceThread serviceThread))
                {
                    var newServiceThread = LifetimeScope.Resolve<ILuaInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("filePath", filePath)
                    );

                    newServiceThread.Start();

                    Instances.Add(instanceId, newServiceThread);
                }
            }

            return LifetimeScope.Resolve<ILuaLuaReference>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("serviceThread", Instances[instanceId])
            );
        }
    }
}