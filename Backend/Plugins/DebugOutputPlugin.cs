using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class DebugOutputPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "DebugOutputPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; internal set; }
        public string WorkerName => "Core";
        private IEventBusSubscription? EventBusSubscription;
        private readonly EventHandler EventHandler = new EventHandler();

        public DebugOutputPlugin(string id)
        {
            Id = id;

            EventHandler.OnDefault += EventHandler_OnDefault;
            EventHandler.OnInternalPluginStateChanged += EventHandler_OnInternalPluginStateChanged;
            EventHandler.OnInternalPluginRegister += EventHandler_OnInternalPluginRegister;
            EventHandler.OnInternalPluginUnregister += EventHandler_OnInternalPluginUnregister;
            EventHandler.OnInternalPluginEnable += EventHandler_OnInternalPluginEnable;
            EventHandler.OnInternalPluginDisable += EventHandler_OnInternalPluginDisable;
            EventHandler.OnUtilityWriteToConsole += EventHandler_OnUtilityWriteToConsole;
            EventHandler.OnUtilitySay += EventHandler_OnUtilitySay;
            EventHandler.OnUtilityPlayAudio += EventHandler_OnUtilityPlayAudio;
        }

        private void EventHandler_OnUtilityPlayAudio(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.PlayAudio> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} message=\"{ev.Filename}\", volume={ev.Volume}");
        }

        private void EventHandler_OnInternalPluginDisable(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginDisable> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginEnable(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginEnable> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginUnregister(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginUnregister> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginRegister(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginRegister> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id} pluginname={ev.PluginName}, enabled={ev.Enabled}");
        }

        private void EventHandler_OnUtilitySay(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.Say> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} message=\"{ev.Message}\" @ volume={ev.Volume}");
        }

        private void EventHandler_OnUtilityWriteToConsole(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.WriteToConsole> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Message}");
        }

        private void EventHandler_OnInternalPluginStateChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginStateChanged> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id} {ev.PluginName} {ev.PluginStatus}");
        }

        private void EventHandler_OnDefault(EventHandler source, EventHandler.EventHandlerArgs<IEvent> e)
        {
            Debug.WriteLine($"$$ {e.Event}");
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
        }

        public void RegisterPlugin(IEngine engine)
        {
            EventBusSubscription = engine.RegisterListener();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            EventBusSubscription?.Dispose();
            EventBusSubscription = null;
        }

        public void Loop()
        {
            var e = EventBusSubscription?.NextEvent(250);

            if (Enabled)
            {
                EventHandler.HandleEvent(e);
            }
        }
    }
}
