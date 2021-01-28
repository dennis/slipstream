using NLua;
using Serilog;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Components;
using Slipstream.Shared;
using System;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Backend.Services
{
    internal class LuaService : ILuaSevice
    {
        private readonly ILogger Logger;
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;
        private readonly IEventSerdeService EventSerdeService;
        private readonly Lua Lua;

        public LuaService(
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IStateService stateService,
            IEventSerdeService eventSerdeService,
            List<ILuaGlue>? luaGlues = null)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            EventSerdeService = eventSerdeService;
            Lua = new Lua();

            luaGlues ??= new List<ILuaGlue>();

            foreach (var glue in luaGlues)
            {
                glue.SetupLua(Lua);
            }
        }

        public ILuaContext Parse(string filename, string logPrefix)
        {
            return new LuaContext(Logger, EventFactory, EventBus, StateService, EventSerdeService, filename, logPrefix, Lua);
        }
    }
}