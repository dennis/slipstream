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

        private volatile bool enabled = true;
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        public delegate void OnDefaultHandler(EventHandler source, EventHandlerArgs<IEvent> e);
        public event OnDefaultHandler? OnDefault;

        #region Events: Internal
        public delegate void OnInternalCommandPluginRegisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.CommandPluginRegister> e);
        public event OnInternalCommandPluginRegisterHandler? OnInternalCommandPluginRegister;

        public delegate void OnInternalCommandPluginUnregisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.CommandPluginUnregister> e);
        public event OnInternalCommandPluginUnregisterHandler? OnInternalCommandPluginUnregister;

        public delegate void OnInternalCommandPluginEnableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.CommandPluginEnable> e);
        public event OnInternalCommandPluginEnableHandler? OnInternalCommandPluginEnable;

        public delegate void OnInternalCommandPluginDisableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.CommandPluginDisable> e);
        public event OnInternalCommandPluginDisableHandler? OnInternalCommandPluginDisable;

        public delegate void OnInternalPluginStateHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginState> e);
        public event OnInternalPluginStateHandler? OnInternalPluginState;

        public delegate void OnInternalFileMonitorFileCreatedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileCreated> e);
        public event OnInternalFileMonitorFileCreatedHandler? OnInternalFileMonitorFileCreated;

        public delegate void OnInternalFileMonitorFileChangedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileChanged> e);
        public event OnInternalFileMonitorFileChangedHandler? OnInternalFileMonitorFileChanged;

        public delegate void OnInternalFileMonitorFileDeletedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileDeleted> e);
        public event OnInternalFileMonitorFileDeletedHandler? OnInternalFileMonitorFileDeleted;

        public delegate void OnInternalFileMonitorFileRenamedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.FileMonitorFileRenamed> e);
        public event OnInternalFileMonitorFileRenamedHandler? OnInternalFileMonitorFileRenamed;

        public delegate void OnInternalPluginsReadyHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.PluginsReady> e);
        public event OnInternalPluginsReadyHandler? OnInternalPluginsReady;

        public delegate void OnInternalCommandPluginStatesHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.CommandPluginStates> e);
        public event OnInternalCommandPluginStatesHandler? OnInternalCommandPluginStates;

        #endregion

        #region Events: Utility
        public delegate void OnUtilityCommandWriteToConsoleHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.CommandWriteToConsole> e);
        public event OnUtilityCommandWriteToConsoleHandler? OnUtilityCommandWriteToConsole;

        public delegate void OnUtilityCommandSayHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.CommandSay> e);
        public event OnUtilityCommandSayHandler? OnUtilityCommandSay;

        public delegate void OnUtilityCommandPlayAudioHandler(EventHandler source, EventHandlerArgs<Shared.Events.Utility.CommandPlayAudio> e);
        public event OnUtilityCommandPlayAudioHandler? OnUtilityCommandPlayAudio;
        #endregion

        #region Events: Setting
        public delegate void OnSettingFileMonitorSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.FileMonitorSettings> e);
        public event OnSettingFileMonitorSettingsHandler? OnSettingFileMonitorSettings;

        public delegate void OnSettingAudioSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.AudioSettings> e);
        public event OnSettingAudioSettingsHandler? OnSettingAudioSettings;

        public delegate void OnSettingTwitchSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.TwitchSettings> e);
        public event OnSettingTwitchSettingsHandler? OnSettingTwitchSettings;

        public delegate void OnSettingLuaSettingsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Setting.LuaSettings> e);
        public event OnSettingLuaSettingsHandler? OnSettingLuaSettings;
        #endregion

        #region Events: IRacing
        public delegate void OnIRacingConnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingConnected> e);
        public event OnIRacingConnectedHandler? OnIRacingConnected;

        public delegate void OnIRacingDisconnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingDisconnected> e);
        public event OnIRacingDisconnectedHandler? OnIRacingDisconnected;

        public delegate void OnIRacingTrackInfoHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingTrackInfo> e);
        public event OnIRacingTrackInfoHandler? OnIRacingTrackInfo;

        public delegate void OnIRacingWeatherInfoHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingWeatherInfo> e);
        public event OnIRacingWeatherInfoHandler? OnIRacingWeatherInfo;

        public delegate void OnIRacingCurrentSessionHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingCurrentSession> e);
        public event OnIRacingCurrentSessionHandler? OnIRacingCurrentSession;

        public delegate void OnIRacingCarInfoHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingCarInfo> e);
        public event OnIRacingCarInfoHandler? OnIRacingCarInfo;

        public delegate void OnIRacingRaceFlagsHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingRaceFlags> e);
        public event OnIRacingRaceFlagsHandler? OnIRacingRaceFlags;

        public delegate void OnIRacingSessionStateHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingSessionState> e);
        public event OnIRacingSessionStateHandler? OnIRacingSessionState;

        public delegate void OnIRacingCarCompletedLapHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingCarCompletedLap> e);
        public event OnIRacingCarCompletedLapHandler? OnIRacingCarCompletedLap;

        public delegate void OnIRacingPitEnterHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingPitEnter> e);
        public event OnIRacingPitEnterHandler? OnIRacingPitEnter;

        public delegate void OnIRacingPitExitHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingPitExit> e);
        public event OnIRacingPitExitHandler? OnIRacingPitExit;

        public delegate void OnIRacingPitstopReportHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingPitstopReport> e);
        public event OnIRacingPitstopReportHandler? OnIRacingPitstopReport;
        #endregion

        #region Events: Twitch
        public delegate void OnTwitchConnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchConnected> e);
        public event OnTwitchConnectedHandler? OnTwitchConnected;

        public delegate void OnTwitchDisconnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchDisconnected> e);
        public event OnTwitchDisconnectedHandler? OnTwitchDisconnected;

        public delegate void OnTwitchReceivedCommandHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchReceivedCommand> e);
        public event OnTwitchReceivedCommandHandler? OnTwitchReceivedCommand;

        public delegate void OnTwitchCommandSendMessageHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.CommandTwitchSendMessage> e);
        public event OnTwitchCommandSendMessageHandler? OnTwitchCommandSendMessage;
        #endregion

        public void HandleEvent(IEvent? ev)
        {
            if (ev == null || !Enabled)
                return;

            switch (ev)
            {
                case null:
                    // ignore
                    break;

                // Internal

                case Shared.Events.Internal.CommandPluginRegister tev:
                    if (OnInternalCommandPluginRegister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginRegister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.CommandPluginRegister>(tev));
                    break;
                case Shared.Events.Internal.CommandPluginUnregister tev:
                    if (OnInternalCommandPluginUnregister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginUnregister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.CommandPluginUnregister>(tev));
                    break;
                case Shared.Events.Internal.CommandPluginEnable tev:
                    if (OnInternalCommandPluginEnable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginEnable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.CommandPluginEnable>(tev));
                    break;
                case Shared.Events.Internal.CommandPluginDisable tev:
                    if (OnInternalCommandPluginDisable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginDisable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.CommandPluginDisable>(tev));
                    break;
                case Shared.Events.Internal.PluginState tev:
                    if (OnInternalPluginState == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginState.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginState>(tev));
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
                case Shared.Events.Internal.PluginsReady tev:
                    if (OnInternalPluginsReady == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginsReady.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.PluginsReady>(tev));
                    break;
                case Shared.Events.Internal.CommandPluginStates tev:
                    if (OnInternalCommandPluginStates == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginStates.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.CommandPluginStates>(tev));
                    break;

                // Utility

                case Shared.Events.Utility.CommandWriteToConsole tev:
                    if (OnUtilityCommandWriteToConsole == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUtilityCommandWriteToConsole.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.CommandWriteToConsole>(tev));
                    break;

                case Shared.Events.Utility.CommandSay tev:
                    if (OnUtilityCommandSay == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUtilityCommandSay.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.CommandSay>(tev));
                    break;

                case Shared.Events.Utility.CommandPlayAudio tev:
                    if (OnUtilityCommandPlayAudio == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUtilityCommandPlayAudio.Invoke(this, new EventHandlerArgs<Shared.Events.Utility.CommandPlayAudio>(tev));
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

                case Shared.Events.Setting.TwitchSettings tev:
                    if (OnSettingTwitchSettings == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnSettingTwitchSettings.Invoke(this, new EventHandlerArgs<Shared.Events.Setting.TwitchSettings>(tev));
                    break;

                case Shared.Events.Setting.LuaSettings tev:
                    if (OnSettingLuaSettings == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnSettingLuaSettings.Invoke(this, new EventHandlerArgs<Shared.Events.Setting.LuaSettings>(tev));
                    break;

                // IRacing

                case Shared.Events.IRacing.IRacingConnected tev:
                    if (OnIRacingConnected == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingConnected.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingConnected>(tev));
                    break;

                case Shared.Events.IRacing.IRacingDisconnected tev:
                    if (OnIRacingDisconnected == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingDisconnected.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingDisconnected>(tev));
                    break;

                case Shared.Events.IRacing.IRacingTrackInfo tev:
                    if (OnIRacingTrackInfo == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingTrackInfo.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingTrackInfo>(tev));
                    break;

                case Shared.Events.IRacing.IRacingWeatherInfo tev:
                    if (OnIRacingWeatherInfo == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingWeatherInfo.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingWeatherInfo>(tev));
                    break;

                case Shared.Events.IRacing.IRacingCurrentSession tev:
                    if (OnIRacingCurrentSession == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingCurrentSession.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingCurrentSession>(tev));
                    break;

                case Shared.Events.IRacing.IRacingCarInfo tev:
                    if (OnIRacingCarInfo == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingCarInfo.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingCarInfo>(tev));
                    break;

                case Shared.Events.IRacing.IRacingRaceFlags tev:
                    if (OnIRacingRaceFlags == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingRaceFlags.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingRaceFlags>(tev));
                    break;

                case Shared.Events.IRacing.IRacingSessionState tev:
                    if (OnIRacingSessionState == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingSessionState.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingSessionState>(tev));
                    break;

                case Shared.Events.IRacing.IRacingCarCompletedLap tev:
                    if (OnIRacingCarCompletedLap == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingCarCompletedLap.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingCarCompletedLap>(tev));
                    break;

                case Shared.Events.IRacing.IRacingPitEnter tev:
                    if (OnIRacingPitEnter == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingPitEnter.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingPitEnter>(tev));
                    break;

                case Shared.Events.IRacing.IRacingPitExit tev:
                    if (OnIRacingPitExit == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingPitExit.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingPitExit>(tev));
                    break;

                case Shared.Events.IRacing.IRacingPitstopReport tev:
                    if (OnIRacingPitstopReport == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIRacingPitstopReport.Invoke(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingPitstopReport>(tev));
                    break;

                // Twitch

                case Shared.Events.Twitch.TwitchConnected tev:
                    if (OnTwitchConnected == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnTwitchConnected.Invoke(this, new EventHandlerArgs<Shared.Events.Twitch.TwitchConnected>(tev));
                    break;

                case Shared.Events.Twitch.TwitchDisconnected tev:
                    if (OnTwitchDisconnected == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnTwitchDisconnected.Invoke(this, new EventHandlerArgs<Shared.Events.Twitch.TwitchDisconnected>(tev));
                    break;

                case Shared.Events.Twitch.TwitchReceivedCommand tev:
                    if (OnTwitchReceivedCommand == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnTwitchReceivedCommand.Invoke(this, new EventHandlerArgs<Shared.Events.Twitch.TwitchReceivedCommand>(tev));
                    break;

                case Shared.Events.Twitch.CommandTwitchSendMessage tev:
                    if (OnTwitchCommandSendMessage == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnTwitchCommandSendMessage.Invoke(this, new EventHandlerArgs<Shared.Events.Twitch.CommandTwitchSendMessage>(tev));
                    break;

                default:
                    throw new Exception($"Unknown event '{ev}");
            }
        }
    }
}
