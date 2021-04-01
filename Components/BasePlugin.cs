#nullable enable

using Slipstream.Shared;
using System;

namespace Slipstream.Components
{
    public class BasePlugin : IPlugin
    {
        public string Id { get; } = "INVALID-PLUGIN-ID";
        private string name = "INVALID-PLUGIN-NAME";

        public string Name
        {
            get { return name; }
            set { name = value; OnStateChanged?.Invoke(this, this); }
        }

        private string displayName = "INVALID-DISPLAY-NAME";

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnStateChanged?.Invoke(this, this); }
        }

        public event EventHandler<IPlugin>? OnStateChanged;

        public bool Reconfigurable { get; }
        public bool FullThreadControl { get; }

        public IEventHandlerController EventHandlerController { get; }

        public BasePlugin(IEventHandlerController eventHandlerController, string id, string name, string displayName, bool reconfigurable = false, bool fullThreadControl = false)
        {
            EventHandlerController = eventHandlerController;
            Id = id;
            Name = name;
            DisplayName = displayName;
            Reconfigurable = reconfigurable;
            FullThreadControl = fullThreadControl;
        }

        public virtual void Run()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}