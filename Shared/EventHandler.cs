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

        #region Events: Internal
        public delegate void OnInternalPluginRegisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginRegister> e);
        public event OnInternalPluginRegisterHandler? OnInternalPluginRegister;

        public delegate void OnInternalPluginUnregisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginUnregister> e);
        public event OnInternalPluginUnregisterHandler? OnInternalPluginUnregister;

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

        public delegate void OnInternalPluginsReadyHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginsReady> e);
        public event OnInternalPluginsReadyHandler? OnInternalPluginsReady;

        #endregion

        #region Events: Utility
        public delegate void OnUtilityWriteToConsoleHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.WriteToConsole> e);
        public event OnUtilityWriteToConsoleHandler? OnUtilityWriteToConsole;

        public delegate void OnUtilitySayHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.Say> e);
        public event OnUtilitySayHandler? OnUtilitySay;

        public delegate void OnPlayAudioHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.PlayAudio> e);
        public event OnPlayAudioHandler? OnPlayAudio;
        #endregion

        #region Events: Setting
        public delegate void OnSettingFileMonitorSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.FileMonitorSettings> e);
        public event OnSettingFileMonitorSettingsHandler? OnSettingFileMonitorSettings;

        public delegate void OnSettingAudioSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.AudioSettings> e);
        public event OnSettingAudioSettingsHandler? OnSettingAudioSettings;
        #endregion

        public void HandleEvent(IEvent? ev)
        {
            switch (ev)
            {
                case null:
                    // ignore
                    break;

                // Internal

                case Shared.Events.Internal.PluginRegister tev:
                    if (OnInternalPluginRegister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginRegister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginRegister>(tev));
                    break;
                case Shared.Events.Internal.PluginUnregister tev:
                    if (OnInternalPluginUnregister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginUnregister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginUnregister>(tev));
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
                case Shared.Events.Internal.PluginsReady tev:
                    if (OnInternalPluginsReady == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginsReady.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginsReady>(tev));
                    break;

                // Utility

                case Shared.Events.Utility.WriteToConsole tev:
                    if (OnUtilityWriteToConsole == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUtilityWriteToConsole.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.WriteToConsole>(tev));
                    break;

                case Shared.Events.Utility.Say tev:
                    if (OnUtilitySay == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUtilitySay.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.Say>(tev));
                    break;

                case Shared.Events.Utility.PlayAudio tev:
                    if (OnPlayAudio == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnPlayAudio.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.PlayAudio>(tev));
                    break;

                // Setting

                case Shared.Events.Setting.FileMonitorSettings tev:
                    if (OnSettingFileMonitorSettings == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnSettingFileMonitorSettings.Invoke(this, new EventHandlerArgs<Shared.Events.Setting.FileMonitorSettings>(tev));
                    break;

                case Shared.Events.Setting.AudioSettings tev:
                    if (OnSettingAudioSettings == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnSettingAudioSettings.Invoke(this, new EventHandlerArgs<Shared.Events.Setting.AudioSettings>(tev));
                    break;

                default:
                    throw new Exception($"Unknown event '{ev}");
            }
        }
    }
}
