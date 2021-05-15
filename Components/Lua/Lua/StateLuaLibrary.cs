#nullable enable

using Autofac;
using NLua;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class StateLuaLibrary : ILuaLibraryAutoRegistration
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

        public ILuaReference? instance(LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);
            var instanceId = cfg.Extract<string>("id");

            return LifetimeScope.Resolve<StateLuaReference>(
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("luaLibrary", this)
            );
        }

        public void ReferenceDropped(ILuaReference luaReference)
        {
        }
    }
}