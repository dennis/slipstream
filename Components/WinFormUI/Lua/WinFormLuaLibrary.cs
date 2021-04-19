#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.Collections.Generic;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormLuaLibrary : ILuaLibrary
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        private readonly object Lock = new object();
        private readonly ILifetimeScope LifetimeScope;
        private readonly Dictionary<string, IWinFormInstanceThread> Instances = new Dictionary<string, IWinFormInstanceThread>();

        public string Name => "api/winformui";

        static WinFormLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id");
        }

        public WinFormLuaLibrary(ILifetimeScope scope)
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

            lock (Lock)
            {
                if (!Instances.TryGetValue(instanceId, out IWinFormInstanceThread instance))
                {
                    var newInstance = LifetimeScope.Resolve<IWinFormInstanceThread>(
                        new NamedParameter("instanceId", instanceId)
                    );
                    newInstance.Start();

                    Instances.Add(instanceId, newInstance);
                }

                return LifetimeScope.Resolve<IWinFormReference>(
                    new NamedParameter("instanceId", instanceId)
                );
            }
        }
    }
}