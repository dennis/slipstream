using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slipstream.UnitTests.TestData
{
    class CarInfoDataSampleTestData : IEnumerable<object[]>
    {
        private const int MAXCARS = 64;
        private readonly List<DataSample> data;

        public CarInfoDataSampleTestData()
        {
            data = new List<DataSample>
            {
                CreateCarInfoTestData(1),
                CreateCarInfoTestData(1, userId: 1),
                CreateCarInfoTestData(1, userId: 2, userName: "Dennis")
            };
        }

        private DataSample CreateCarInfoTestData(
            long carIdx,
            double sessionTime = 0,
            string carNumber = "not set",
            long userId = -1,
            string userName = "Slipstream Jones",
            long teamId = -1,
            string teamName = "SSR Red",
            string carScreenName = "Snail Speed Racing Red",
            string carScreenNameShort = "SSR Red",
            long iRating = 1350,
            string licString = "A4.99",
            long driverCarIdx = -1,
            long isSpectator = 0,
            CarState carState = null
            )
        {
            DataSample data = CreateDataSample();

            // data.IsConnected = true; 
            // need to cheat here to set the property as it is marked for internal set in iRacingSDK
            typeof(DataSample).GetProperty("IsConnected").SetValue(data, true); 
            data.Telemetry["SessionTime"] = sessionTime;
            data.SessionData.DriverInfo.Drivers[carIdx].CarIdx = carIdx;
            data.SessionData.DriverInfo.Drivers[carIdx].CarNumber = carNumber;
            data.SessionData.DriverInfo.Drivers[carIdx].UserID = userId;
            data.SessionData.DriverInfo.Drivers[carIdx].UserName = userName;
            data.SessionData.DriverInfo.Drivers[carIdx].TeamID = teamId;
            data.SessionData.DriverInfo.Drivers[carIdx].TeamName= teamName;
            data.SessionData.DriverInfo.Drivers[carIdx].CarScreenName = carScreenName;
            data.SessionData.DriverInfo.Drivers[carIdx].CarScreenNameShort = carScreenNameShort;
            data.SessionData.DriverInfo.Drivers[carIdx].IRating = iRating;
            data.SessionData.DriverInfo.Drivers[carIdx].LicString = licString;
            data.SessionData.DriverInfo.DriverCarIdx = driverCarIdx;
            data.SessionData.DriverInfo.Drivers[carIdx].IsSpectator = isSpectator;

            SetCarState(carIdx, data, carState ?? new CarState());

            return data;
        }

        private void SetCarState(long carIdx, DataSample data, CarState carState)
        {
            data.Telemetry["CarIdxLap"] = new int[MAXCARS];
            data.Telemetry["CarIdxLapCompleted"] = new int[MAXCARS];

            data.Telemetry.CarIdxLapCompleted[carIdx] = carState.StintStartLap;
            data.Telemetry["FuelLevel"] = carState.StintFuelLevel;
            data.Telemetry["SessionTime"] = carState.StintStartTime;
        }

        private static DataSample CreateDataSample()
        {
            var driverInfo = new SessionData._DriverInfo();
            driverInfo.Drivers = Enumerable.Repeat(new SessionData._DriverInfo._Drivers(), MAXCARS).ToArray(); // Default to 64 drivers, can be reduced if needed

            var sessionData = new SessionData
            {
                CameraInfo = new SessionData._CameraInfo(),
                DriverInfo = driverInfo,
                RadioInfo = new SessionData._RadioInfo(),
                SessionInfo = new SessionData._SessionInfo(),
                SplitTimeInfo = new SessionData._SplitTimeInfo(),
                WeekendInfo = new SessionData._WeekendInfo()
            };

            var telemetry = new Telemetry();
            
            return new DataSample
            {
                SessionData = sessionData,
                Telemetry = telemetry,
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return data.Select(m => new [] {m}).GetEnumerator(); // Create an object array with the data sample to be used by TestTheory data in tests as params
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
