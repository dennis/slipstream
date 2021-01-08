using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class LuaService : ILuaSevice
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;

        public LuaService(IEventFactory eventFactory, IEventBus eventBus, IStateService stateService)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
        }

        public ILuaContext Parse(string filename, string logPrefix)
        {
            return new LuaContext(EventFactory, EventBus, StateService, filename, logPrefix);
        }
    }
}
