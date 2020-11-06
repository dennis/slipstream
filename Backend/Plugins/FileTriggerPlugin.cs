using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileTriggerPlugin : Worker, IPlugin, IEventListener
    {
        public string Name => "FileTriggerPlugin";

        public bool Enabled { get; internal set; }
        private IEventBusSubscription? EventBusSubscription;
        private readonly IEventBus EventBus;
        private IDictionary<string, bool> Scripts = new Dictionary<string, bool>();

        public FileTriggerPlugin(IEventBus eventBus)
        {
            this.EventBus = eventBus;
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
            EventHandler eventHandler = new EventHandler();

            eventHandler.OnInternalFileMonitorFileCreated += EventHandler_OnInternalFileMonitorFileCreated;
            eventHandler.OnInternalFileMonitorFileDeleted += EventHandler_OnInternalFileMonitorFileDeleted;
            eventHandler.OnInternalFileMonitorFileChanged += EventHandler_OnInternalFileMonitorFileChanged;
            eventHandler.OnInternalFileMonitorFileRenamed += EventHandler_OnInternalFileMonitorFileRenamed;

            while (!Stopped)
            {
                var e = EventBusSubscription?.NextEvent(250);

                if (Enabled)
                {
                    eventHandler.HandleEvent(e);
                }
            }
        }

        private void EventHandler_OnInternalFileMonitorFileRenamed(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileRenamed> e)
        {
            throw new System.NotImplementedException();
        }

        private void EventHandler_OnInternalFileMonitorFileChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileChanged> e)
        {
            throw new System.NotImplementedException();
        }

        private void EventHandler_OnInternalFileMonitorFileDeleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileDeleted> e)
        {
            throw new System.NotImplementedException();
        }

        private void EventHandler_OnInternalFileMonitorFileCreated(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileCreated> e)
        {
            if (e.Event.FilePath == null)
                return;

            Debug.WriteLine($"New file seen! {e.Event.FilePath}");

            Scripts.Add(e.Event.FilePath, true);
        }
    }
}
