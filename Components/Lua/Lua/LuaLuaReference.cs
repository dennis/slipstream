#nullable enable

using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class LuaLuaReference : BaseLuaReference, ILuaLuaReference
    {
        private readonly ILuaInstanceThread ServiceThread;

        public LuaLuaReference(string instanceId, string luaScriptInstanceId, ILuaInstanceThread serviceThread) : base(instanceId, luaScriptInstanceId)
        {
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

        public void join()
        {
            ServiceThread.Join();
        }
    }
}