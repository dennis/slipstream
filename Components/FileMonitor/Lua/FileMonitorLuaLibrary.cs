#nullable enable

using Autofac;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Slipstream.Shared.Lua;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaLibrary : CommodityLuaLibrary<IFileMonitorInstanceThread, IFileMonitorLuaReference>
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        static FileMonitorLuaLibrary()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("id")
                .RequireArray("paths", (a) => a.RequireString());
        }

        public FileMonitorLuaLibrary(ILifetimeScope scope) : base(ConfigurationValidator, scope)
        {
        }

        protected override IFileMonitorInstanceThread CreateInstance(ILifetimeScope scope, Parameters cfg)
        {
            string instanceId = cfg.Extract<String>("id");
            string[] paths = (cfg.Extract<Dictionary<dynamic, dynamic>>("paths").Values.Cast<string>()!).ToArray();

            return scope.Resolve<IFileMonitorInstanceThread>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("paths", paths)
            );
        }
    }
}