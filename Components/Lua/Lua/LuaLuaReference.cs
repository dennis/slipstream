#nullable enable

using System;

namespace Slipstream.Components.Lua.Lua
{
    public class LuaLuaReference : ILuaLuaReference
    {
        private readonly ILuaInstanceThread ServiceThread;

        public LuaLuaReference(ILuaInstanceThread serviceThread)
        {
            ServiceThread = serviceThread;
        }

        public void stop()
        {
            ServiceThread.Stop();
        }

        public void Dispose()
        {
        }
    }
}