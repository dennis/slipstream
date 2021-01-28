#nullable enable

using Slipstream.Shared;

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

        public event IPlugin.OnStateChangedHandler? OnStateChanged;

        public bool Reconfigurable { get; }

        public IEventHandlerController EventHandlerController { get; }

        public BasePlugin(IEventHandlerController eventHandlerController, string id, string name, string displayName, bool reconfigurable = false)
        {
            EventHandlerController = eventHandlerController;
            Id = id;
            Name = name;
            DisplayName = displayName;
            Reconfigurable = reconfigurable;
        }

        public virtual void Loop()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}