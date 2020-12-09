﻿using System;
using static Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class BasePlugin : IPlugin
    {
        public string Id { get;  } = "INVALID-PLUGIN-ID";
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

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; OnStateChanged?.Invoke(this, new IPlugin.EventHandlerArgs<IPlugin>(this)); }
        }

        private string workerName = "INVALID-WORKER-NAME";

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

        public void Disable(IEngine engine)
        {
            Enabled = false;
            EventHandler.Enabled = false;

            OnDisable(engine);
        }

        protected virtual void OnDisable(IEngine engine)
        {
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
            EventHandler.Enabled = true;

            OnEnable(engine);
        }

        protected virtual void OnEnable(IEngine engine)
        {
        }

        public virtual void Loop()
        {
        }

        public void RegisterPlugin(IEngine engine)
        {
            OnRegisterPlugin(engine);
        }

        protected virtual void OnRegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
            OnUnregisterPlugin(engine);
        }

        protected virtual void OnUnregisterPlugin(IEngine engine)
        {
        }
    }
}