#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class UtilLuaLibrary : ILuaLibraryAutoRegistration
    {
        private readonly ILifetimeScope LifetimeScope;

        public string Name => "api/util";

        public UtilLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);
            var instanceId = cfg.Extract<string>("id");

            return LifetimeScope.Resolve<UtilLuaReference>(
                new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("luaLibrary", this)
            );
        }
    }
}