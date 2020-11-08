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

        public DebugOutputPlugin(string id)
        {
            Id = id;
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
            if (Enabled && e != null)
            {
                if (e is Slipstream.Shared.Events.Internal.PluginStateChanged ev)
                {
                    Debug.WriteLine($"DebugOutputPlugin got event: {e} {ev.Id} {ev.PluginName} {ev.PluginStatus}");
                }
                else
                {
                    Debug.WriteLine($"DebugOutputPlugin got event: {e}");
                }

            }
        }
    }
}
