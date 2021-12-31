#nullable enable

using Slipstream.Components.IRacing.Events;
using Slipstream.Shared;

using System;

namespace Slipstream.Components.IRacing.EventHandler
{
    internal class IRacing : IEventHandler
    {
        public event EventHandler<IRacingCompletedLap>? OnIRacingCarCompletedLap;

        public event EventHandler<IRacingCarInfo>? OnIRacingCarInfo;

        public event EventHandler<IRacingCommandSendCarInfo>? OnIRacingCommandSendCarInfo;

        public event EventHandler<IRacingCommandSendRaceFlags>? OnIRacingCommandSendRaceFlags;

        public event EventHandler<IRacingCommandSendSessionState>? OnIRacingCommandSendSessionState;

        public event EventHandler<IRacingCommandSendTrackInfo>? OnIRacingCommandSendTrackInfo;

        public event EventHandler<IRacingCommandSendWeatherInfo>? OnIRacingCommandSendWeatherInfo;

        public event EventHandler<IRacingConnected>? OnIRacingConnected;

        public event EventHandler<IRacingDisconnected>? OnIRacingDisconnected;

        public event EventHandler<IRacingDriverIncident>? OnIRacingDriverIncident;

        public event EventHandler<IRacingPitEnter>? OnIRacingPitEnter;

        public event EventHandler<IRacingPitExit>? OnIRacingPitExit;

        public event EventHandler<IRacingPitstopReport>? OnIRacingPitstopReport;

        public event EventHandler<IRacingRaceFlags>? OnIRacingRaceFlags;

        public event EventHandler<IRacingTrackInfo>? OnIRacingTrackInfo;

        public event EventHandler<IRacingWeatherInfo>? OnIRacingWeatherInfo;

        public event EventHandler<IRacingPractice>? OnIRacingPractice;

        public event EventHandler<IRacingQualify>? OnIRacingQualify;

        public event EventHandler<IRacingRace>? OnIRacingRace;

        public event EventHandler<IRacingTesting>? OnIRacingTesting;

        public event EventHandler<IRacingWarmup>? OnIRacingWarmup;

        public event EventHandler<IRacingCarPosition>? OnIRacingCarPosition;

        public event EventHandler<IRacingRaw>? OnIRacingRaw;

        public event EventHandler<IRacingTowed>? OnIRacingTowed;

        public event EventHandler<IRacingTrackPosition>? OnIRacingTrackPosition;

        public event EventHandler<IRacingCommandPitChangeLeftFrontTyre>? OnIRacingCommandPitChangeLeftFrontTyre;

        public event EventHandler<IRacingCommandPitChangeRightFrontTyre>? OnIRacingCommandPitChangeRightFrontTyre;

        public event EventHandler<IRacingCommandPitChangeLeftRearTyre>? OnIRacingCommandPitChangeLeftRearTyre;

        public event EventHandler<IRacingCommandPitChangeRightRearTyre>? OnIRacingCommandPitChangeRightRearTyre;

        public event EventHandler<IRacingCommandPitClearAll>? OnIRacingCommandPitClearAll;

        public event EventHandler<IRacingCommandPitClearTyresChange>? OnIRacingCommandPitClearTyresChange;

        public event EventHandler<IRacingCommandPitAddFuel>? OnIRacingCommandPitAddFuel;

        public event EventHandler<IRacingCommandPitRequestFastRepair>? OnIRacingCommandPitRequestFastRepair;

        public event EventHandler<IRacingCommandPitCleanWindshield>? OnIRacingCommandPitCleanWindshield;

        public event EventHandler<IRacingTime>? OnIRacingTime;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                IRacingConnected tev => OnEvent(OnIRacingConnected, tev),
                IRacingDisconnected tev => OnEvent(OnIRacingDisconnected, tev),
                IRacingTrackInfo tev => OnEvent(OnIRacingTrackInfo, tev),
                IRacingWeatherInfo tev => OnEvent(OnIRacingWeatherInfo, tev),
                IRacingCarInfo tev => OnEvent(OnIRacingCarInfo, tev),
                IRacingRaceFlags tev => OnEvent(OnIRacingRaceFlags, tev),
                IRacingCompletedLap tev => OnEvent(OnIRacingCarCompletedLap, tev),
                IRacingPitEnter tev => OnEvent(OnIRacingPitEnter, tev),
                IRacingPitExit tev => OnEvent(OnIRacingPitExit, tev),
                IRacingPitstopReport tev => OnEvent(OnIRacingPitstopReport, tev),
                IRacingCommandSendCarInfo tev => OnEvent(OnIRacingCommandSendCarInfo, tev),
                IRacingCommandSendTrackInfo tev => OnEvent(OnIRacingCommandSendTrackInfo, tev),
                IRacingCommandSendWeatherInfo tev => OnEvent(OnIRacingCommandSendWeatherInfo, tev),
                IRacingCommandSendSessionState tev => OnEvent(OnIRacingCommandSendSessionState, tev),
                IRacingCommandSendRaceFlags tev => OnEvent(OnIRacingCommandSendRaceFlags, tev),
                IRacingDriverIncident tev => OnEvent(OnIRacingDriverIncident, tev),
                IRacingPractice tev => OnEvent(OnIRacingPractice, tev),
                IRacingQualify tev => OnEvent(OnIRacingQualify, tev),
                IRacingRace tev => OnEvent(OnIRacingRace, tev),
                IRacingTesting tev => OnEvent(OnIRacingTesting, tev),
                IRacingWarmup tev => OnEvent(OnIRacingWarmup, tev),
                IRacingCarPosition tev => OnEvent(OnIRacingCarPosition, tev),
                IRacingRaw tev => OnEvent(OnIRacingRaw, tev),
                IRacingTowed tev => OnEvent(OnIRacingTowed, tev),
                IRacingTrackPosition tev => OnEvent(OnIRacingTrackPosition, tev),
                IRacingCommandPitChangeLeftFrontTyre tev => OnEvent(OnIRacingCommandPitChangeLeftFrontTyre, tev),
                IRacingCommandPitChangeRightFrontTyre tev => OnEvent(OnIRacingCommandPitChangeRightFrontTyre, tev),
                IRacingCommandPitChangeLeftRearTyre tev => OnEvent(OnIRacingCommandPitChangeLeftRearTyre, tev),
                IRacingCommandPitChangeRightRearTyre tev => OnEvent(OnIRacingCommandPitChangeRightRearTyre, tev),
                IRacingCommandPitClearAll tev => OnEvent(OnIRacingCommandPitClearAll, tev),
                IRacingCommandPitClearTyresChange tev => OnEvent(OnIRacingCommandPitClearTyresChange, tev),
                IRacingCommandPitAddFuel tev => OnEvent(OnIRacingCommandPitAddFuel, tev),
                IRacingCommandPitRequestFastRepair tev => OnEvent(OnIRacingCommandPitRequestFastRepair, tev),
                IRacingCommandPitCleanWindshield tev => OnEvent(OnIRacingCommandPitCleanWindshield, tev),
                IRacingTime tev => OnEvent(OnIRacingTime, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}