using System;

namespace Slipstream.Backend.Plugins
{
    public class BasePlugin : IPlugin
    {
        public bool DirtyState { get; set; }

        public string Id
        {
            get { return Id; }
            set { Id = value; DirtyState = true; }
        }

        public string Name
        {
            get { return Name; }
            set { Name = value; DirtyState = true; }
        }

        public string DisplayName
        {
            get { return DisplayName; }
            set { DisplayName = value; DirtyState = true; }
        }

        public bool Enabled
        {
            get { return Enabled; }
            set { Enabled = value; DirtyState = true; }
        }

        public string WorkerName
        {
            get { return WorkerName; }
            set { WorkerName = value; DirtyState = true; }
        }

        public Shared.EventHandler EventHandler => throw new NotImplementedException();

        public BasePlugin(string id, string name, string displayName, string workerName)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            WorkerName = workerName;
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = false;
        }

        public void Loop()
        {
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }
    }
}
