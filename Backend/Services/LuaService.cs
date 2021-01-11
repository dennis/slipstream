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
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IAudioEventFactory AudioEventFactory;
        private readonly IIRacingEventFactory IRacingEventFactory;
        private readonly IUIEventFactory UIEventFactory;
        private readonly ITwitchEventFactory TwitchEventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;

        public LuaService(
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IStateService stateService)
        {
            Logger = logger;
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            AudioEventFactory = eventFactory.Get<IAudioEventFactory>();
            UIEventFactory = eventFactory.Get<IUIEventFactory>();
            TwitchEventFactory = eventFactory.Get<ITwitchEventFactory>();
            IRacingEventFactory = eventFactory.Get<IIRacingEventFactory>();
            EventBus = eventBus;
            StateService = stateService;
        }

        public ILuaContext Parse(string filename, string logPrefix)
        {
            return new LuaContext(Logger, AudioEventFactory, IRacingEventFactory, InternalEventFactory, UIEventFactory, TwitchEventFactory, EventBus, StateService, filename, logPrefix);
        }
    }
}
