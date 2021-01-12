using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class LuaPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly ILuaEventFactory LuaEventFactory;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly CapturingEventBus EventBus;
        private readonly LuaService LuaService;
        private ILuaContext? LuaContext;
        private readonly string Prefix = "<UNKNOWN>";
        private readonly string FilePath;

        public LuaPlugin(string id,
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            IStateService stateService,
            ILuaConfiguration configuration
        ) : base(id, "LuaPlugin", "LuaPlugin", "Lua")
        {
            Logger = logger;
            LuaEventFactory = eventFactory.Get<ILuaEventFactory>();
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            EventBus = new CapturingEventBus(eventBus);

            LuaService = new LuaService(logger, eventFactory, EventBus, stateService);

            // Avoid that WriteToConsole is evaluated by Lua, that in turn will
            // add more WriteToConsole events, making a endless loop
            EventHandler.OnUICommandWriteToConsole += (s, e) => { };
            EventHandler.OnDefault += (s, e) => LuaContext?.HandleEvent(e.Event);

            FilePath = configuration.FilePath;
            Prefix = Path.GetFileName(FilePath);

            StartLua();
        }

        private void StartLua()
        {
            DisplayName = "Lua: " + Path.GetFileName(FilePath);

            try
            {
                LuaContext = LuaService.Parse(FilePath, Prefix);
            }
            catch (LuaException e)
            {
                HandleLuaException(e);
            }

            var eventsCaptured = EventBus.CapturedEvents;
            EventBus.StopCapturing();

            EventBus.PublishEvent(LuaEventFactory.CreateLuaManagerCommandDeduplicateEvents(eventsCaptured));
        }

        private void HandleLuaException(LuaException e)
        {
            Logger.Error("Lua error: {Message}", e.Message);
            EventBus.PublishEvent(InternalEventFactory.CreateInternalCommandPluginUnregister(Id));
        }

        public override void Loop()
        {
            try
            {
                LuaContext?.Loop();
            }
            catch (LuaException e)
            {
                HandleLuaException(e);
            }
        }
    }
}