#nullable enable

using System;

namespace Slipstream.Shared
{
    public class EventHandler
    {
        public class EventHandlerArgs<T> : EventArgs
        {
            public T Event { get; }

            public EventHandlerArgs(T e)
            {
                Event = e;
            }
        }

        public delegate void OnDefaultHandler(EventHandler source, EventHandlerArgs<IEvent> e);
        public event OnDefaultHandler? OnDefault;

        public delegate void OnInternalPluginLoadHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginLoad> e);
        public event OnInternalPluginLoadHandler? OnInternalPluginLoad;

        public delegate void OnInternalPluginEnableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginEnable> e);
        public event OnInternalPluginEnableHandler? OnInternalPluginEnable;

        public delegate void OnInternalPluginDisableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginDisable> e);
        public event OnInternalPluginDisableHandler? OnInternalPluginDisable;

        public delegate void OnInternalPluginStateChangedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginStateChanged> e);
        public event OnInternalPluginStateChangedHandler? OnInternalPluginStateChanged;

        public delegate void OnInternalFileMonitorFileCreatedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileCreated> e);
        public event OnInternalFileMonitorFileCreatedHandler? OnInternalFileMonitorFileCreated;

        public delegate void OnInternalFileMonitorFileChangedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileChanged> e);
        public event OnInternalFileMonitorFileChangedHandler? OnInternalFileMonitorFileChanged;

        public delegate void OnInternalFileMonitorFileDeletedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileDeleted> e);
        public event OnInternalFileMonitorFileDeletedHandler? OnInternalFileMonitorFileDeleted;

        public delegate void OnInternalFileMonitorFileRenamedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileRenamed> e);
        public event OnInternalFileMonitorFileRenamedHandler? OnInternalFileMonitorFileRenamed;

        public delegate void OnInternalFrontendReadyHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FrontendReady> e);
        public event OnInternalFrontendReadyHandler? OnInternalFrontendReady;

        public void HandleEvent(IEvent? ev)
        {
            switch (ev)
            {
                case null:
                    // ignore
                    break;
                case Shared.Events.Internal.PluginLoad tev:
                    if(OnInternalPluginLoad == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginLoad.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginLoad>(tev));
                    break;
                case Shared.Events.Internal.PluginEnable tev:
                    if (OnInternalPluginEnable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginEnable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginEnable>(tev));
                    break;
                case Shared.Events.Internal.PluginDisable tev:
                    if (OnInternalPluginDisable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginDisable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginDisable>(tev));
                    break;
                case Shared.Events.Internal.PluginStateChanged tev:
                    if (OnInternalPluginStateChanged == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginStateChanged.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginStateChanged>(tev));
                    break;
                case Shared.Events.Internal.FileMonitorFileCreated tev:
                    if (OnInternalFileMonitorFileCreated == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalFileMonitorFileCreated.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.FileMonitorFileCreated>(tev));
                    break;
                case Shared.Events.Internal.FileMonitorFileChanged tev:
                    if (OnInternalFileMonitorFileChanged == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalFileMonitorFileChanged.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.FileMonitorFileChanged>(tev));
                    break;
                case Shared.Events.Internal.FileMonitorFileDeleted tev:
                    if (OnInternalFileMonitorFileDeleted == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalFileMonitorFileDeleted.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.FileMonitorFileDeleted>(tev));
                    break;
                case Shared.Events.Internal.FileMonitorFileRenamed tev:
                    if (OnInternalFileMonitorFileRenamed == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalFileMonitorFileRenamed.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.FileMonitorFileRenamed>(tev));
                    break;
                case Shared.Events.Internal.FrontendReady tev:
                    if (OnInternalFrontendReady == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalFrontendReady.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.FrontendReady>(tev));
                    break;
                default:
                    throw new Exception($"Unknown event '{ev}");
            }
        }
    }
}
