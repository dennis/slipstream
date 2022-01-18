#nullable enable

using NLua;

using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaLibrary : IDisposable
    {
        string Name { get; }

        ILuaReference? GetInstance(string luaScriptInstanceId, LuaTable cfg);
    }
}