using NLua.Exceptions;
using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Components.UI.EventHandler;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System.IO;

#nullable enable

namespace Slipstream.Components.Lua.Plugins
{
    public class LuaPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly ILuaEventFactory LuaEventFactory;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly CapturingEventBus EventBus;
        private readonly ILuaSevice LuaService;
        private ILuaContext? LuaContext;
        private readonly string Prefix = "<UNKNOWN>";
        private readonly string FilePath;

        public LuaPlugin(
            IEventHandlerController eventHandlerController,
            string id,
            ILogger logger,
            IEventFactory eventFactory,
            IEventBus eventBus,
            ILuaSevice luaService,
            Parameters configuration
        ) : base(eventHandlerController, id, "LuaPlugin", id)
        {
            Logger = logger;
            LuaEventFactory = eventFactory.Get<ILuaEventFactory>();
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            LuaService = luaService;
            EventBus = new CapturingEventBus(eventBus);

            // Avoid that WriteToConsole is evaluated by Lua, that in turn will
            // add more WriteToConsole events, making a endless loop
            EventHandlerController.Get<UIEventHandler>().OnUICommandWriteToConsole += (s, e) => { };
            EventHandlerController.OnDefault += (s, e) => LuaContext?.HandleEvent(e.Event);

            FilePath = configuration.Extract<string>("filepath");

            StartLua();
        }

        private void StartLua()
        {
            DisplayName = "Lua: " + Path.GetFileName(FilePath);

            try
            {
                LuaContext = LuaService.Parse(FilePath);
            }
            catch (LuaException e)
            {
                HandleLuaException(e);
            }

            var eventsCaptured = EventBus.CapturedEvents;
            EventBus.StopCapturing();

            EventBus.PublishEvent(LuaEventFactory.CreateLuaCommandDeduplicateEvents(eventsCaptured));
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
                LuaService.Loop();
            }
            catch (LuaException e)
            {
                HandleLuaException(e);
            }
        }
    }
}