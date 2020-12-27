using System;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class BasePlugin : IPlugin
    {
        public string Id { get; } = "INVALID-PLUGIN-ID";
        private string name = "INVALID-PLUGIN-NAME";
        public string Name
        {
            get { return name; }
            set { name = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        private string displayName = "INVALID-DISPLAY-NAME";
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        private volatile bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        private string workerName = "INVALID-WORKER-NAME";

        private volatile bool pendingOnEnable;
        private volatile bool pendingOnDisable;
        public bool PendingOnEnable { get { return pendingOnEnable; } set { pendingOnEnable = value; } }
        public bool PendingOnDisable { get { return pendingOnDisable; } set { pendingOnDisable = value; } }

        public event IPlugin.OnStateChangedHandler? OnStateChanged;

        public string WorkerName
        {
            get { return workerName; }
            set { workerName = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        public Shared.EventHandler EventHandler { get; } = new Shared.EventHandler();

        public BasePlugin(string id, string name, string displayName, string workerName)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            WorkerName = workerName;
        }

        public void Disable()
        {
            Enabled = false;
            EventHandler.Enabled = false;

            PendingOnEnable = false;
            PendingOnDisable = true;
        }

        public virtual void OnDisable()
        {
        }

        public void Enable()
        {
            Enabled = true;
            EventHandler.Enabled = true;

            PendingOnEnable = true;
            PendingOnDisable = false;
        }

        public virtual void OnEnable()
        {
        }

        public virtual void Loop()
        {
        }

        public void Dispose()
        {
        }
    }
}
