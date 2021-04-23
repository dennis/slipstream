#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class InternalLuaLibrary : ILuaLibrary
    {
        private readonly ILifetimeScope LifetimeScope;

        public string Name => "api/internal";

        public InternalLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable luaTable)
        {
            return LifetimeScope.Resolve<InternalLuaReference>();
        }
    }
}