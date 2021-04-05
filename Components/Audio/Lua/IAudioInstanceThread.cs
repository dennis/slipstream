#nullable enable

using System;

namespace Slipstream.Components.Audio.Lua
{
    public interface IAudioInstanceThread : IDisposable
    {
        void Start();
    }
}