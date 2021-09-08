#nullable enable

using System;

namespace Slipstream.Shared.Lua
{
    public interface ILuaInstanceThread : IDisposable
    {
        bool Stopped { get; }

        void Start();

        void Stop();
    }
}