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

        public void Dispose()
        {
            LuaLibrary.ReferenceDrop(this);
        }

        public void join()
        {
            ServiceThread.Join();
        }
    }
}