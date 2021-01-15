using NLua;
using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
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

        public LuaContext(
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IStateService stateService,
            IEventSerdeService eventSerdeService,
            string filePath,
            string logPrefix
        )
        {
            try
            {
                Lua = new Lua();

                var audioEventFactory = eventFactory.Get<IAudioEventFactory>();
                var twitchEventFactory = eventFactory.Get<ITwitchEventFactory>();
                var uiEventFactory = eventFactory.Get<IUIEventFactory>();
                var internalEventFactory = eventFactory.Get<IInternalEventFactory>();
                var iRacingEventFactory = eventFactory.Get<IIRacingEventFactory>();
                var playbackEventFactory = eventFactory.Get<IPlaybackEventFactory>();

                CoreMethodCollection_ = CoreMethodCollection.Register(eventSerdeService, Lua);
                AudioMethodCollection.Register(eventBus, audioEventFactory, Lua);
                TwitchMethodCollection.Register(eventBus, twitchEventFactory, Lua);
                StateMethodCollection.Register(stateService, Lua);
                UIMethodCollection.Register(logger, eventBus, uiEventFactory, logPrefix, Lua);
                InternalMethodCollection.Register(eventBus, internalEventFactory, Lua);
                HttpMethodCollection.Register(logger, Lua);
                IRacingMethodCollection.Register(eventBus, iRacingEventFactory, Lua);
                PlaybackMethodCollection.Register(eventBus, playbackEventFactory, Lua);

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