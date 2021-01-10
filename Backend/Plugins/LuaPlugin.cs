using Slipstream.Backend.Services;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class LuaPlugin : BasePlugin
    {
        private readonly IEventFactory EventFactory;
        private readonly CapturingEventBus EventBus;
        private readonly LuaService LuaService;
        private ILuaContext? LuaContext;
        private readonly string Prefix = "<UNKNOWN>";
        private readonly string FilePath;

        public LuaPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, ILuaConfiguration configuration) : base(id, "LuaPlugin", "LuaPlugin", "Lua")
        {
            EventFactory = eventFactory;
            EventBus = new CapturingEventBus(eventBus);

            LuaService = new LuaService(eventFactory, EventBus, stateService);

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

            EventBus.PublishEvent(EventFactory.CreateLuaManagerCommandDeduplicateEvents(eventsCaptured));
        }

        private void HandleLuaException(LuaException e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"{Prefix}: ERROR: { e.Message}"));
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister(Id));
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