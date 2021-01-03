using Slipstream.Backend.Services;
using Slipstream.Backend.Services.LuaServiceLib;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{

    public class LuaPlugin : BasePlugin
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly LuaService LuaService;
        private ILuaContext? LuaContext;
        private string Prefix = "<UNKNOWN>";
        private string? FilePath;

        public LuaPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, LuaSettings settings) : base(id, "LuaPlugin", "LuaPlugin", "Lua")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            LuaService = new LuaService(eventFactory, eventBus, stateService);

            EventHandler.OnSettingLuaSettings += (s, e) => OnLuaSettings(e.Event);

            // Avoid that WriteToConsole is evaluated by Lua, that in turn will 
            // add more WriteToConsole events, making a endless loop
            EventHandler.OnUICommandWriteToConsole += (s, e) => { };
            EventHandler.OnDefault += (s, e) => LuaContext?.HandleEvent(e.Event);

            OnLuaSettings(settings);
        }

        private void OnLuaSettings(LuaSettings @event)
        {
            if (Id != @event.PluginId)
                return;

            FilePath = @event.FilePath;
            Prefix = Path.GetFileName(FilePath);

            StartLua();
        }

        private void StartLua()
        {
            if (!Enabled || FilePath == null)
                return;

            DisplayName = "Lua: " + Path.GetFileName(FilePath);

            try
            {
                LuaContext = LuaService.Parse(FilePath, Prefix);
            }
            catch (LuaException e)
            {
                HandleLuaException(e);
            }
        }

        private void HandleLuaException(LuaException e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"{Prefix}: ERROR: { e.Message}"));
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister(Id));
        }

        public override void OnDisable()
        {
            LuaContext?.Dispose();
            LuaContext = null;
        }

        public override void OnEnable()
        {
            StartLua();
        }

        public override void Loop()
        {
            if (!Enabled || FilePath == null)
                return;

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
