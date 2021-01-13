using Serilog;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class LuaService : ILuaSevice
    {
        private readonly ILogger Logger;
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;
        private readonly IEventSerdeService EventSerdeService;

        public LuaService(
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IStateService stateService,
            IEventSerdeService eventSerdeService)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            EventSerdeService = eventSerdeService;
        }

        public ILuaContext Parse(string filename, string logPrefix)
        {
            return new LuaContext(Logger, EventFactory, EventBus, StateService, EventSerdeService, filename, logPrefix);
        }
    }
}
