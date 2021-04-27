#nullable enable

namespace Slipstream.Components.Lua.Lua
{
    public class LuaLuaReference : ILuaLuaReference
    {
        private readonly ILuaInstanceThread ServiceThread;
        private readonly LuaLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public LuaLuaReference(LuaLuaLibrary luaLibrary, string instanceId, ILuaInstanceThread serviceThread)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
            ServiceThread = serviceThread;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void stop()
        {
            ServiceThread.Stop();
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDrop(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void join()
        {
            ServiceThread.Join();
        }
    }
}