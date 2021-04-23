#nullable enable

using Autofac;
using NLua;
using Serilog;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class UtilLuaLibrary : ILuaLibrary
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

        public ILuaReference? instance(LuaTable cfg)
        {
            return LifetimeScope.Resolve<UtilLuaReference>();
        }
    }
}