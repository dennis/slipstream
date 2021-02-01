#nullable enable

using Slipstream.Components.IRacing.Events;
using Slipstream.Shared;

namespace Slipstream.Components.IRacing.EventHandler
{
    internal class IRacing : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public IRacing(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnIRacingCarCompletedLapHandler(EventHandlerController source, EventHandlerArgs<IRacingCarCompletedLap> e);

        public delegate void OnIRacingCarInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingCarInfo> e);

        public delegate void OnIRacingCommandSendCarInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingCommandSendCarInfo> e);

        public delegate void OnIRacingCommandSendRaceFlagsHandler(EventHandlerController source, EventHandlerArgs<IRacingCommandSendRaceFlags> e);

        public delegate void OnIRacingCommandSendSessionStateHandler(EventHandlerController source, EventHandlerArgs<IRacingCommandSendSessionState> e);

        public delegate void OnIRacingCommandSendTrackInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingCommandSendTrackInfo> e);

        public delegate void OnIRacingCommandSendWeatherInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingCommandSendWeatherInfo> e);

        public delegate void OnIRacingConnectedHandler(EventHandlerController source, EventHandlerArgs<IRacingConnected> e);

        public delegate void OnIRacingDisconnectedHandler(EventHandlerController source, EventHandlerArgs<IRacingDisconnected> e);

        public delegate void OnIRacingDriverIncidentHandler(EventHandlerController source, EventHandlerArgs<IRacingDriverIncident> e);

        public delegate void OnIRacingPitEnterHandler(EventHandlerController source, EventHandlerArgs<IRacingPitEnter> e);

        public delegate void OnIRacingPitExitHandler(EventHandlerController source, EventHandlerArgs<IRacingPitExit> e);

        public delegate void OnIRacingPitstopReportHandler(EventHandlerController source, EventHandlerArgs<IRacingPitstopReport> e);

        public delegate void OnIRacingRaceFlagsHandler(EventHandlerController source, EventHandlerArgs<IRacingRaceFlags> e);

        public delegate void OnIRacingTrackInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingTrackInfo> e);

        public delegate void OnIRacingWeatherInfoHandler(EventHandlerController source, EventHandlerArgs<IRacingWeatherInfo> e);

        public delegate void OnIRacingPracticeHandler(EventHandlerController source, EventHandlerArgs<IRacingPractice> e);

        public delegate void OnIRacingQualifyHandler(EventHandlerController source, EventHandlerArgs<IRacingQualify> e);

        public delegate void OnIRacingRaceHandler(EventHandlerController source, EventHandlerArgs<IRacingRace> e);

        public delegate void OnIRacingTestingHandler(EventHandlerController source, EventHandlerArgs<IRacingTesting> e);

        public delegate void OnIRacingWarmupHandler(EventHandlerController source, EventHandlerArgs<IRacingWarmup> e);

        public delegate void OnIRacingCarPositionHandler(EventHandlerController source, EventHandlerArgs<IRacingCarPosition> e);

        public event OnIRacingCarCompletedLapHandler? OnIRacingCarCompletedLap;

        public event OnIRacingCarInfoHandler? OnIRacingCarInfo;

        public event OnIRacingCommandSendCarInfoHandler? OnIRacingCommandSendCarInfo;

        public event OnIRacingCommandSendRaceFlagsHandler? OnIRacingCommandSendRaceFlags;

        public event OnIRacingCommandSendSessionStateHandler? OnIRacingCommandSendSessionState;

        public event OnIRacingCommandSendTrackInfoHandler? OnIRacingCommandSendTrackInfo;

        public event OnIRacingCommandSendWeatherInfoHandler? OnIRacingCommandSendWeatherInfo;

        public event OnIRacingConnectedHandler? OnIRacingConnected;

        public event OnIRacingDisconnectedHandler? OnIRacingDisconnected;

        public event OnIRacingDriverIncidentHandler? OnIRacingDriverIncident;

        public event OnIRacingPitEnterHandler? OnIRacingPitEnter;

        public event OnIRacingPitExitHandler? OnIRacingPitExit;

        public event OnIRacingPitstopReportHandler? OnIRacingPitstopReport;

        public event OnIRacingRaceFlagsHandler? OnIRacingRaceFlags;

        public event OnIRacingTrackInfoHandler? OnIRacingTrackInfo;

        public event OnIRacingWeatherInfoHandler? OnIRacingWeatherInfo;

        public event OnIRacingPracticeHandler? OnIRacingPractice;

        public event OnIRacingQualifyHandler? OnIRacingQualify;

        public event OnIRacingRaceHandler? OnIRacingRace;

        public event OnIRacingTestingHandler? OnIRacingTesting;

        public event OnIRacingWarmupHandler? OnIRacingWarmup;

        public event OnIRacingCarPositionHandler? OnIRacingCarPosition;

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
                case IRacingPractice tev:
                    if (OnIRacingPractice != null)
                    {
                        OnIRacingPractice(Parent, new EventHandlerArgs<IRacingPractice>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingQualify tev:
                    if (OnIRacingQualify != null)
                    {
                        OnIRacingQualify(Parent, new EventHandlerArgs<IRacingQualify>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingRace tev:
                    if (OnIRacingRace != null)
                    {
                        OnIRacingRace(Parent, new EventHandlerArgs<IRacingRace>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingTesting tev:
                    if (OnIRacingTesting != null)
                    {
                        OnIRacingTesting(Parent, new EventHandlerArgs<IRacingTesting>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingWarmup tev:
                    if (OnIRacingWarmup != null)
                    {
                        OnIRacingWarmup(Parent, new EventHandlerArgs<IRacingWarmup>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case IRacingCarPosition tev:
                    if (OnIRacingCarPosition != null)
                    {
                        OnIRacingCarPosition(Parent, new EventHandlerArgs<IRacingCarPosition>(tev));
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