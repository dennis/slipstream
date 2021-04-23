#nullable enable

using NLua;
using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaLibrary : IDisposable
    {
        string Name { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        ILuaReference? instance(LuaTable cfg);
    }
}