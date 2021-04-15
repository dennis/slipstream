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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void stop()
        {
            ServiceThread.Stop();
        }

        public void Dispose()
        {
        }
    }
}