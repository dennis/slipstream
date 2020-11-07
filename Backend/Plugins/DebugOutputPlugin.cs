using Slipstream.Shared;
using System;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class DebugOutputPlugin : Worker, IPlugin, IEventListener
    {
        public Guid Id { get; set; }
        public string Name => "DebugOutputPlugin";
        public string DisplayName => Name;

        public bool Enabled { get; internal set; }
        private IEventBusSubscription? EventBusSubscription;

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

            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            EventBusSubscription?.Dispose();
            EventBusSubscription = null;

            Stop();
        }

        protected override void Main()
        {
            while (!Stopped)
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
}
