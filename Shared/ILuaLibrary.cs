#nullable enable

using NLua;
using System;

namespace Slipstream.Shared
{
    public interface ILuaLibrary : IDisposable
    {
        string Name { get; }

        ILuaReference? instance(LuaTable cfg);
    }
}