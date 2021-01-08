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
        public delegate void OnInternalCommandPluginRegisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginRegister> e);
        public event OnInternalCommandPluginRegisterHandler? OnInternalCommandPluginRegister;

        public delegate void OnInternalCommandPluginUnregisterHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginUnregister> e);
        public event OnInternalCommandPluginUnregisterHandler? OnInternalCommandPluginUnregister;

        public delegate void OnInternalCommandPluginEnableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginEnable> e);
        public event OnInternalCommandPluginEnableHandler? OnInternalCommandPluginEnable;

        public delegate void OnInternalCommandPluginDisableHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginDisable> e);
        public event OnInternalCommandPluginDisableHandler? OnInternalCommandPluginDisable;

        public delegate void OnInternalPluginStateHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalPluginState> e);
        public event OnInternalPluginStateHandler? OnInternalPluginState;

        public delegate void OnInternalCommandPluginStatesHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginStates> e);
        public event OnInternalCommandPluginStatesHandler? OnInternalCommandPluginStates;

        public delegate void OnInternalInitializedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalInitialized> e);
        public event OnInternalInitializedHandler? OnInternalInitialized;

        public delegate void OnInternalReconfiguredHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalReconfigured> e);
        public event OnInternalReconfiguredHandler? OnInternalReconfigured;

        public delegate void OnInternalBootupEventsHandler(EventHandler source, EventHandlerArgs<Shared.Events.Internal.InternalBootupEvents> e);
        public event OnInternalBootupEventsHandler? OnInternalBootupEvents;

        #endregion

        #region FileMonitor
        public delegate void OnFileMonitorFileCreatedHandler(EventHandler source, EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileCreated> e);
        public event OnFileMonitorFileCreatedHandler? OnFileMonitorFileCreated;

        public delegate void OnFileMonitorFileChangedHandler(EventHandler source, EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileChanged> e);
        public event OnFileMonitorFileChangedHandler? OnFileMonitorFileChanged;

        public delegate void OnFileMonitorFileDeletedHandler(EventHandler source, EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileDeleted> e);
        public event OnFileMonitorFileDeletedHandler? OnFileMonitorFileDeleted;

        public delegate void OnFileMonitorFileRenamedHandler(EventHandler source, EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileRenamed> e);
        public event OnFileMonitorFileRenamedHandler? OnFileMonitorFileRenamed;

        public delegate void OnFileMonitorScanCompletedHandler(EventHandler source, EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorScanCompleted> e);
        public event OnFileMonitorScanCompletedHandler? OnFileMonitorScanCompleted;
        #endregion

        #region Events: UI
        public delegate void OnUICommandWriteToConsoleHandler(EventHandler source, EventHandlerArgs<Shared.Events.UI.UICommandWriteToConsole> e);
        public event OnUICommandWriteToConsoleHandler? OnUICommandWriteToConsole;

        public delegate void OnUICommandCreateButtonHandler(EventHandler source, EventHandlerArgs<Shared.Events.UI.UICommandCreateButton> e);
        public event OnUICommandCreateButtonHandler? OnUICommandCreateButton;

        public delegate void OnUICommandDeleteButtonHandler(EventHandler source, EventHandlerArgs<Shared.Events.UI.UICommandDeleteButton> e);
        public event OnUICommandDeleteButtonHandler? OnUICommandDeleteButton;

        public delegate void OnUIButtonTriggeredHandler(EventHandler source, EventHandlerArgs<Shared.Events.UI.UIButtonTriggered> e);
        public event OnUIButtonTriggeredHandler? OnUIButtonTriggered;
        #endregion

        #region Events: Audio
        public delegate void OnAudioCommandSayHandler(EventHandler source, EventHandlerArgs<Shared.Events.Audio.AudioCommandSay> e);
        public event OnAudioCommandSayHandler? OnAudioCommandSay;

        public delegate void OnAudioCommandPlayHandler(EventHandler source, EventHandlerArgs<Shared.Events.Audio.AudioCommandPlay> e);
        public event OnAudioCommandPlayHandler? OnAudioCommandPlay;
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

        public delegate void OnIracingDriverIncidentHandler(EventHandler source, EventHandlerArgs<Shared.Events.IRacing.IRacingDriverIncident> e);
        public event OnIracingDriverIncidentHandler? OnIracingDriverIncident;
        #endregion

        #region Events: Twitch
        public delegate void OnTwitchConnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchConnected> e);
        public event OnTwitchConnectedHandler? OnTwitchConnected;

        public delegate void OnTwitchDisconnectedHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchDisconnected> e);
        public event OnTwitchDisconnectedHandler? OnTwitchDisconnected;

        public delegate void OnTwitchReceivedCommandHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchReceivedCommand> e);
        public event OnTwitchReceivedCommandHandler? OnTwitchReceivedCommand;

        public delegate void OnTwitchCommandSendMessageHandler(EventHandler source, EventHandlerArgs<Shared.Events.Twitch.TwitchCommandSendMessage> e);
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

                case Shared.Events.Internal.InternalCommandPluginRegister tev:
                    if (OnInternalCommandPluginRegister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginRegister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginRegister>(tev));
                    break;
                case Shared.Events.Internal.InternalCommandPluginUnregister tev:
                    if (OnInternalCommandPluginUnregister == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginUnregister.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginUnregister>(tev));
                    break;
                case Shared.Events.Internal.InternalCommandPluginEnable tev:
                    if (OnInternalCommandPluginEnable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginEnable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginEnable>(tev));
                    break;
                case Shared.Events.Internal.InternalCommandPluginDisable tev:
                    if (OnInternalCommandPluginDisable == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginDisable.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginDisable>(tev));
                    break;
                case Shared.Events.Internal.InternalPluginState tev:
                    if (OnInternalPluginState == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalPluginState.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalPluginState>(tev));
                    break;
                case Shared.Events.Internal.InternalCommandPluginStates tev:
                    if (OnInternalCommandPluginStates == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalCommandPluginStates.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalCommandPluginStates>(tev));
                    break;
                case Shared.Events.Internal.InternalInitialized tev:
                    if (OnInternalInitialized == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalInitialized.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalInitialized>(tev));
                    break;
                    
                case Shared.Events.Internal.InternalReconfigured tev:
                    if (OnInternalReconfigured == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalReconfigured.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalReconfigured>(tev));
                    break;

                case Shared.Events.Internal.InternalBootupEvents tev:
                    if (OnInternalBootupEvents == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnInternalBootupEvents.Invoke(this, new EventHandlerArgs<Shared.Events.Internal.InternalBootupEvents>(tev));
                    break;

                // File Monitor

                case Shared.Events.FileMonitor.FileMonitorFileCreated tev:
                    if (OnFileMonitorFileCreated == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnFileMonitorFileCreated.Invoke(this, new EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileCreated>(tev));
                    break;
                case Shared.Events.FileMonitor.FileMonitorFileChanged tev:
                    if (OnFileMonitorFileChanged == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnFileMonitorFileChanged.Invoke(this, new EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileChanged>(tev));
                    break;
                case Shared.Events.FileMonitor.FileMonitorFileDeleted tev:
                    if (OnFileMonitorFileDeleted == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnFileMonitorFileDeleted.Invoke(this, new EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileDeleted>(tev));
                    break;
                case Shared.Events.FileMonitor.FileMonitorFileRenamed tev:
                    if (OnFileMonitorFileRenamed == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnFileMonitorFileRenamed.Invoke(this, new EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileRenamed>(tev));
                    break;
                case Shared.Events.FileMonitor.FileMonitorScanCompleted tev:
                    if (OnFileMonitorScanCompleted == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnFileMonitorScanCompleted.Invoke(this, new EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorScanCompleted>(tev));
                    break;

                // Audio

                case Shared.Events.Audio.AudioCommandSay tev:
                    if (OnAudioCommandSay == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnAudioCommandSay.Invoke(this, new EventHandlerArgs<Shared.Events.Audio.AudioCommandSay>(tev));
                    break;

                case Shared.Events.Audio.AudioCommandPlay tev:
                    if (OnAudioCommandPlay == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnAudioCommandPlay.Invoke(this, new EventHandlerArgs<Shared.Events.Audio.AudioCommandPlay>(tev));
                    break;

                // UI

                case Shared.Events.UI.UICommandWriteToConsole tev:
                    if (OnUICommandWriteToConsole == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUICommandWriteToConsole.Invoke(this, new EventHandlerArgs<Shared.Events.UI.UICommandWriteToConsole>(tev));
                    break;

                case Shared.Events.UI.UICommandCreateButton tev:
                    if (OnUICommandCreateButton == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUICommandCreateButton.Invoke(this, new EventHandlerArgs<Shared.Events.UI.UICommandCreateButton>(tev));
                    break;

                case Shared.Events.UI.UICommandDeleteButton tev:
                    if (OnUICommandDeleteButton == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUICommandDeleteButton.Invoke(this, new EventHandlerArgs<Shared.Events.UI.UICommandDeleteButton>(tev));
                    break;

                case Shared.Events.UI.UIButtonTriggered tev:
                    if (OnUIButtonTriggered == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnUIButtonTriggered.Invoke(this, new EventHandlerArgs<Shared.Events.UI.UIButtonTriggered>(tev));
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

                case Shared.Events.IRacing.IRacingDriverIncident tev:
                    if (OnIracingDriverIncident == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnIracingDriverIncident(this, new EventHandlerArgs<Shared.Events.IRacing.IRacingDriverIncident>(tev));
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

                case Shared.Events.Twitch.TwitchCommandSendMessage tev:
                    if (OnTwitchCommandSendMessage == null)
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(tev));
                    else
                        OnTwitchCommandSendMessage.Invoke(this, new EventHandlerArgs<Shared.Events.Twitch.TwitchCommandSendMessage>(tev));
                    break;

                default:
                    throw new Exception($"Unknown event '{ev}");
            }
        }
    }
}
