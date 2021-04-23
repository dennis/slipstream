#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class StateLuaLibrary : ILuaLibrary
    {
        private readonly ILifetimeScope LifetimeScope;

        public string Name => "api/state";

        public StateLuaLibrary(ILifetimeScope scope)
        {
            LifetimeScope = scope;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfg)
        {
            return LifetimeScope.Resolve<StateLuaReference>();
        }
    }
}