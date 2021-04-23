#nullable enable

using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaInstanceThread : IDisposable
    {
        void Start();
    }
}