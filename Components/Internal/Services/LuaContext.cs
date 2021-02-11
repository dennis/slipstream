using Slipstream.Shared;
using System;
using System.IO;

#nullable enable

namespace Slipstream.Components.Internal.Services
{
    public class LuaContext : ILuaContext
    {
        private readonly NLua.LuaFunction? HandleFunc;
        private UInt64 LastLuaGC = 0;

        private NLua.Lua? Lua;

        public LuaContext(
            string filePath,
            NLua.Lua lua
        )
        {
            try
            {
                Lua = lua;

                // Fix paths, so we can require() files relative to where the script is located
                var ScriptPath = Path.GetDirectoryName(filePath).Replace("\\", "\\\\");
                Lua.DoString($"package.path = \"{ScriptPath}\\\\?.lua;\" .. package.path;");

                // Load the LUA
                var f = Lua.LoadFile(filePath);

                // Evalulate it
                f.Call();

                // Find "handle()" function which will be triggered on received events
                HandleFunc = Lua["handle"] as NLua.LuaFunction;
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                throw new LuaException($"Lua: Error initializing {filePath}: {e}", e);
            }
        }

        public void Dispose()
        {
            Lua = null;
        }

        public void HandleEvent(IEvent @event)
        {
            // Perform GC in Lua approx every second
            if (@event.Uptime - LastLuaGC > 1000)
            {
                LastLuaGC = @event.Uptime;
                Lua.DoString("collectgarbage()");
            }

            HandleFunc?.Call(@event);
        }
    }
}