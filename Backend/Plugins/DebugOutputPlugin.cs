using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class DebugOutputPlugin : Worker, IPlugin, IEventListener
    {
        public string Name => "DebugOutputPlugin";

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
            EventBusSubscription = engine.RegisterListener(this);

            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            engine.UnregisterListener(this);

            Stop();
        }

        protected override void Main()
        {
            while (!Stopped)
            {
                var e = EventBusSubscription?.NextEvent(250);
                if (Enabled && e != null)
                {
                    Debug.WriteLine($"DebugOutputPlugin got event: {e}");
                }
            }
        }
    }
}
