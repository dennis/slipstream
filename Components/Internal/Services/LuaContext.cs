using Serilog;
using Slipstream.Shared;
using System.IO;

#nullable enable

namespace Slipstream.Components.Internal.Services
{
    public class LuaContext : ILuaContext
    {
        private readonly NLua.LuaFunction? HandleFunc;

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
            HandleFunc?.Call(@event);
        }
    }
}