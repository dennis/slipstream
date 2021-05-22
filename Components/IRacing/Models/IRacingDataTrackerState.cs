#nullable enable

using Slipstream.Components.IRacing.Events;
using System.Collections.Generic;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.Components.IRacing.Models
{
    internal class IRacingDataTrackerState
    {
        public bool SendTrackInfo { get; set; }
        public bool SendWeatherInfo { get; set; }
        public bool SendCarInfo { get; set; }
        public bool SendRaceFlags { get; set; }
        public bool SendSessionState { get; set; }
        public bool Connected { get; set; }
        public IRacingWeatherInfo? LastWeatherInfo { get; set; }
        public IDictionary<long, CarState> CarsTracked { get; set; } = new Dictionary<long, CarState>();
        public IDictionary<long, LapState> Laps { get; set; } = new Dictionary<long, LapState>();
        public DriverState DriverState_ { get; set; } = new DriverState();
        public IRacingRaceFlags? LastRaceFlags { get; set; }
        public IRacingSessionTypeEnum LastSessionType { get; set; }
        public int[] LastPositionInClass { get; set; } = new int[Constants.MAX_CARS];
        public int[] LastPositionInRace { get; set; } = new int[Constants.MAX_CARS];
        public IRacingSessionStateEnum LastSessionState { get; set; }

        public void FullReset()
        {
            SendTrackInfo = true;
            SendWeatherInfo = true;
            SendRaceFlags = true;
            SendCarInfo = true;
            SendSessionState = true;

            Connected = false;
            LastWeatherInfo = null;
            LastRaceFlags = null;
            CarsTracked.Clear();
            DriverState_.ClearState();
            Laps.Clear();

            for (int i = 0; i < Constants.MAX_CARS; i++)
            {
                LastPositionInClass[i] = -1;
                LastPositionInRace[i] = -1;
            }
        }
    }
}