using Serilog;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class LuaService : ILuaSevice
    {
        private readonly ILogger Logger;
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;

        public LuaService(ILogger logger, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
        }

        public ILuaContext Parse(string filename, string logPrefix)
        {
            return new LuaContext(Logger, EventFactory, EventBus, StateService, filename, logPrefix);
        }
    }
}
