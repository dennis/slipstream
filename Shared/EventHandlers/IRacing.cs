#nullable enable

using Slipstream.Shared.Events.IRacing;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class IRacing : IEventHandler
    {
        private readonly EventHandler Parent;

        public IRacing(EventHandler eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnIRacingCarCompletedLapHandler(EventHandler source, EventHandlerArgs<IRacingCarCompletedLap> e);

        public delegate void OnIRacingCarInfoHandler(EventHandler source, EventHandlerArgs<IRacingCarInfo> e);

        public delegate void OnIRacingCommandSendCarInfoHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendCarInfo> e);

        public delegate void OnIRacingCommandSendCurrentSessionHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendCurrentSession> e);

        public delegate void OnIRacingCommandSendRaceFlagsHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendRaceFlags> e);

        public delegate void OnIRacingCommandSendSessionStateHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendSessionState> e);

        public delegate void OnIRacingCommandSendTrackInfoHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendTrackInfo> e);

        public delegate void OnIRacingCommandSendWeatherInfoHandler(EventHandler source, EventHandlerArgs<IRacingCommandSendWeatherInfo> e);

        public delegate void OnIRacingConnectedHandler(EventHandler source, EventHandlerArgs<IRacingConnected> e);

        public delegate void OnIRacingCurrentSessionHandler(EventHandler source, EventHandlerArgs<IRacingCurrentSession> e);

        public delegate void OnIRacingDisconnectedHandler(EventHandler source, EventHandlerArgs<IRacingDisconnected> e);

        public delegate void OnIRacingDriverIncidentHandler(EventHandler source, EventHandlerArgs<IRacingDriverIncident> e);

        public delegate void OnIRacingPitEnterHandler(EventHandler source, EventHandlerArgs<IRacingPitEnter> e);

        public delegate void OnIRacingPitExitHandler(EventHandler source, EventHandlerArgs<IRacingPitExit> e);

        public delegate void OnIRacingPitstopReportHandler(EventHandler source, EventHandlerArgs<IRacingPitstopReport> e);

        public delegate void OnIRacingRaceFlagsHandler(EventHandler source, EventHandlerArgs<IRacingRaceFlags> e);

        public delegate void OnIRacingSessionStateHandler(EventHandler source, EventHandlerArgs<IRacingSessionState> e);

        public delegate void OnIRacingTrackInfoHandler(EventHandler source, EventHandlerArgs<IRacingTrackInfo> e);

        public delegate void OnIRacingWeatherInfoHandler(EventHandler source, EventHandlerArgs<IRacingWeatherInfo> e);

        public event OnIRacingCarCompletedLapHandler? OnIRacingCarCompletedLap;

        public event OnIRacingCarInfoHandler? OnIRacingCarInfo;

        public event OnIRacingCommandSendCarInfoHandler? OnIRacingCommandSendCarInfo;

        public event OnIRacingCommandSendCurrentSessionHandler? OnIRacingCommandSendCurrentSession;

        public event OnIRacingCommandSendRaceFlagsHandler? OnIRacingCommandSendRaceFlags;

        public event OnIRacingCommandSendSessionStateHandler? OnIRacingCommandSendSessionState;

        public event OnIRacingCommandSendTrackInfoHandler? OnIRacingCommandSendTrackInfo;

        public event OnIRacingCommandSendWeatherInfoHandler? OnIRacingCommandSendWeatherInfo;

        public event OnIRacingConnectedHandler? OnIRacingConnected;

        public event OnIRacingCurrentSessionHandler? OnIRacingCurrentSession;

        public event OnIRacingDisconnectedHandler? OnIRacingDisconnected;

        public event OnIRacingDriverIncidentHandler? OnIRacingDriverIncident;

        public event OnIRacingPitEnterHandler? OnIRacingPitEnter;

        public event OnIRacingPitExitHandler? OnIRacingPitExit;

        public event OnIRacingPitstopReportHandler? OnIRacingPitstopReport;

        public event OnIRacingRaceFlagsHandler? OnIRacingRaceFlags;

        public event OnIRacingSessionStateHandler? OnIRacingSessionState;

        public event OnIRacingTrackInfoHandler? OnIRacingTrackInfo;

        public event OnIRacingWeatherInfoHandler? OnIRacingWeatherInfo;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case IRacingConnected tev:
                    if (OnIRacingConnected != null)
                    {
                        OnIRacingConnected.Invoke(Parent, new EventHandlerArgs<IRacingConnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingDisconnected tev:
                    if (OnIRacingDisconnected != null)
                    {
                        OnIRacingDisconnected.Invoke(Parent, new EventHandlerArgs<IRacingDisconnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingTrackInfo tev:
                    if (OnIRacingTrackInfo != null)
                    {
                        OnIRacingTrackInfo.Invoke(Parent, new EventHandlerArgs<IRacingTrackInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingWeatherInfo tev:
                    if (OnIRacingWeatherInfo != null)
                    {
                        OnIRacingWeatherInfo.Invoke(Parent, new EventHandlerArgs<IRacingWeatherInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCurrentSession tev:
                    if (OnIRacingCurrentSession != null)
                    {
                        OnIRacingCurrentSession.Invoke(Parent, new EventHandlerArgs<IRacingCurrentSession>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCarInfo tev:
                    if (OnIRacingCarInfo != null)
                    {
                        OnIRacingCarInfo.Invoke(Parent, new EventHandlerArgs<IRacingCarInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingRaceFlags tev:
                    if (OnIRacingRaceFlags != null)
                    {
                        OnIRacingRaceFlags.Invoke(Parent, new EventHandlerArgs<IRacingRaceFlags>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingSessionState tev:
                    if (OnIRacingSessionState != null)
                    {
                        OnIRacingSessionState.Invoke(Parent, new EventHandlerArgs<IRacingSessionState>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCarCompletedLap tev:
                    if (OnIRacingCarCompletedLap != null)
                    {
                        OnIRacingCarCompletedLap.Invoke(Parent, new EventHandlerArgs<IRacingCarCompletedLap>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingPitEnter tev:
                    if (OnIRacingPitEnter != null)
                    {
                        OnIRacingPitEnter.Invoke(Parent, new EventHandlerArgs<IRacingPitEnter>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingPitExit tev:
                    if (OnIRacingPitExit != null)
                    {
                        OnIRacingPitExit.Invoke(Parent, new EventHandlerArgs<IRacingPitExit>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingPitstopReport tev:
                    if (OnIRacingPitstopReport != null)
                    {
                        OnIRacingPitstopReport.Invoke(Parent, new EventHandlerArgs<IRacingPitstopReport>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendCarInfo tev:
                    if (OnIRacingCommandSendCarInfo != null)
                    {
                        OnIRacingCommandSendCarInfo.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendCarInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendTrackInfo tev:
                    if (OnIRacingCommandSendTrackInfo != null)
                    {
                        OnIRacingCommandSendTrackInfo.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendTrackInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendWeatherInfo tev:
                    if (OnIRacingCommandSendWeatherInfo != null)
                    {
                        OnIRacingCommandSendWeatherInfo.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendWeatherInfo>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendCurrentSession tev:
                    if (OnIRacingCommandSendCurrentSession != null)
                    {
                        OnIRacingCommandSendCurrentSession.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendCurrentSession>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendSessionState tev:
                    if (OnIRacingCommandSendSessionState != null)
                    {
                        OnIRacingCommandSendSessionState.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendSessionState>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCommandSendRaceFlags tev:
                    if (OnIRacingCommandSendRaceFlags != null)
                    {
                        OnIRacingCommandSendRaceFlags.Invoke(Parent, new EventHandlerArgs<IRacingCommandSendRaceFlags>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingDriverIncident tev:
                    if (OnIRacingDriverIncident != null)
                    {
                        OnIRacingDriverIncident(Parent, new EventHandlerArgs<IRacingDriverIncident>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}