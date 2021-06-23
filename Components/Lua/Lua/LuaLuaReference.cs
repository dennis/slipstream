#nullable enable

using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class LuaLuaReference : BaseLuaReference, ILuaLuaReference
    {
        private readonly ILuaInstanceThread ServiceThread;
        private readonly LuaLuaLibrary LuaLibrary;

        public LuaLuaReference(LuaLuaLibrary luaLibrary, string instanceId, string luaScriptInstanceId, ILuaInstanceThread serviceThread) : base(instanceId, luaScriptInstanceId)
        {
            LuaLibrary = luaLibrary;
            ServiceThread = serviceThread;
        }

        public void start()
        {
            ServiceThread.Start();
        }

        public void stop()
        {
            ServiceThread.Stop();
        }

        public void restart()
        {
            ServiceThread.Restart();
        }

        public override void Dispose()
        {
            LuaLibrary.ReferenceDrop(this);
        }

        public void join()
        {
            ServiceThread.Join();
        }
    }
}