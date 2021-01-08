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

        private string workerName = "INVALID-WORKER-NAME";

        public event IPlugin.OnStateChangedHandler? OnStateChanged;

        public bool Reconfigurable { get; private set; }

        public string WorkerName
        {
            get { return workerName; }
            set { workerName = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        public Shared.EventHandler EventHandler { get; } = new Shared.EventHandler();

        public BasePlugin(string id, string name, string displayName, string workerName, bool reconfigurable = false)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            WorkerName = workerName;
            Reconfigurable = reconfigurable;
        }

        public virtual void Loop()
        {
        }

        public void Dispose()
        {
        }
    }
}