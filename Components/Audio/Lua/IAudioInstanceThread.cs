#nullable enable

using Slipstream.Shared.Lua;
using System;

namespace Slipstream.Components.Audio.Lua
{
    public interface IAudioInstanceThread : ILuaInstanceThread, IDisposable
    {
    }
}