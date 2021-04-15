using Serilog;
using Slipstream.Components.Internal.LuaGlues;
using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Internal.Plugins
{
    public class InternalPlugin : BasePlugin, IPlugin
    {
        private readonly IInternalEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly ILogger Logger;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IStateService StateService;
        private readonly ILuaLibraryRepository ProviderClassRepository;

        public InternalPlugin(
            IEventHandlerController eventHandlerController,
            string id,
            ILogger logger,
            IEventBus eventBus,
            IInternalEventFactory eventFactory,
            IEventSerdeService eventSerdeService,
            IStateService stateService,
            ILuaLibraryRepository providerClassRepository)
            : base(eventHandlerController, id, nameof(InternalPlugin), id, true)
        {
            EventFactory = eventFactory;
            Logger = logger;
            EventBus = eventBus;
            EventSerdeService = eventSerdeService;
            StateService = stateService;
            ProviderClassRepository = providerClassRepository;
        }

        public IEnumerable<ILuaGlue> CreateLuaGlues()
        {
            return new ILuaGlue[]
            {
                new HttpLuaGlue(Logger),
                new InternalLuaGlue(EventBus, EventFactory),
                new StateLuaGlue(StateService)
            };
        }
    }
}