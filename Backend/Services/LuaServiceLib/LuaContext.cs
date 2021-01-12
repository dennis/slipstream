using NLua;
using Serilog;
using Slipstream.Shared;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext : ILuaContext
    {
        private readonly CoreMethodCollection CoreMethodCollection_;
        private readonly LuaFunction? HandleFunc;
        private Lua? Lua;

        public LuaContext(ILogger logger, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, string filePath, string logPrefix)
        {
            try
            {
                Lua = new Lua();

                CoreMethodCollection_ = CoreMethodCollection.Register(new EventSerdeService(), Lua);
                AudioMethodCollection.Register(eventBus, eventFactory, Lua);
                TwitchMethodCollection.Register(eventBus, eventFactory, Lua);
                StateMethodCollection.Register(stateService, Lua);
                UIMethodCollection.Register(logger, eventBus, eventFactory, logPrefix, Lua);
                InternalMethodCollection.Register(eventBus, eventFactory, Lua);
                HttpMethodCollection.Register(logger, Lua);
                IRacingMethodCollection.Register(eventBus, eventFactory, Lua);

                // Fix paths, so we can require() files relative to where the script is located
                var ScriptPath = Path.GetDirectoryName(filePath).Replace("\\", "\\\\");
                Lua.DoString($"package.path = \"{ScriptPath}\\\\?.lua;\" .. package.path;");

                // Load the LUA
                var f = Lua.LoadFile(filePath);

                // Evalulate it
                f.Call();

                // Find "handle()" function which will be triggered on received events
                HandleFunc = Lua["handle"] as LuaFunction;
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                throw new LuaException($"Error initializing Lua: {e}", e);
            }
        }

        public void Loop()
        {
            try
            {
                CoreMethodCollection_.Loop();
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                throw new LuaException($"Lua runtime error: {e}", e);
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
