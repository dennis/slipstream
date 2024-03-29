﻿#nullable enable

using System;

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
            GC.SuppressFinalize(this);
        }

        public ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfgTable)
        {
            var cfg = Parameters.From(cfgTable);
            var instanceId = cfg.Extract<string>("id");

            return LifetimeScope.Resolve<StateLuaReference>(
                new NamedParameter("luaScriptInstanceId", luaScriptInstanceId),
                new NamedParameter("instanceId", instanceId),
                new NamedParameter("luaLibrary", this)
            );
        }
    }
}