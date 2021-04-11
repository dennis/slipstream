#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaLibrary : ILuaLibrary
    {
        private static readonly DictionaryValidator ConfigurationValidator;
        private readonly object Lock = new object();
        private readonly Dictionary<string, IFileMonitorInstanceThread> Instances = new Dictionary<string, IFileMonitorInstanceThread>();
        private readonly ILifetimeScope LifetimeScope;

        public string Name => "api/filemonitor";

        static FileMonitorLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireArray("paths", (a) => a.RequireString());
        }

        public FileMonitorLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
            foreach (var i in Instances)
            {
                i.Value.Dispose();
            }

            Instances.Clear();
        }

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);

            ConfigurationValidator.Validate(cfg);

            string instanceId = cfg.Extract<String>("id");
            string[] paths = (cfg.Extract<Dictionary<dynamic, dynamic>>("paths").Values.Cast<string>()!).ToArray();

            lock (Lock)
            {
                if (!Instances.TryGetValue(instanceId, out IFileMonitorInstanceThread service))
                {
                    Debug.WriteLine($"Creating instance '{Name}' with id '{instanceId}'");

                    var newService = LifetimeScope.Resolve<IFileMonitorInstanceThread>(
                        new NamedParameter("instanceId", instanceId),
                        new NamedParameter("paths", paths)
                    );

                    newService.Start();

                    Instances.Add(instanceId, newService);
                }

                return LifetimeScope.Resolve<IFileMonitorLuaReference>(
                    new NamedParameter("instanceId", instanceId),
                    new NamedParameter("paths", paths)
                );
            }
        }
    }
}