using iRacingSDK;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class Car
    {
        public int CarIdx { get; set; }
        public string CarNumber { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public long TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public string CarNameShort { get; set; } = string.Empty;
        public long IRating { get; set; }
        public string License { get; set; } = string.Empty;
        public bool IsSpectator { get; set; }
        public int LapsCompleted { get; set; }
        public bool OnPitRoad { get; set; }
        public int ClassPosition { get; set; }
        public int Position { get; set; }

        public Car()
        {
        }

        public Car(int idx, DataSample ds)
        {
            var d = ds.SessionData.DriverInfo.Drivers[idx];

            CarIdx = idx;
            CarNumber = d.CarNumber;
            UserId = d.UserID;
            UserName = d.UserName;
            TeamId = d.TeamID;
            TeamName = d.TeamName;
            CarName = d.CarScreenName;
            CarNameShort = d.CarScreenNameShort;
            IRating = d.IRating;
            License = d.LicString;
            IsSpectator = d.IsSpectator != 0;
            LapsCompleted = ds.Telemetry.CarIdxLapCompleted[idx];
            OnPitRoad = ds.Telemetry.CarIdxOnPitRoad[idx];
            ClassPosition = ds.Telemetry.CarIdxClassPosition[idx];
            Position = ds.Telemetry.CarIdxPosition[idx];
        }
    }
}