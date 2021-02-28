using Slipstream.Components.IRacing.Plugins.GameState;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slipstream.UnitTests.TestData
{
    internal partial class CarInfoDataSampleTestData : IEnumerable<object[]>
    {
        private readonly List<IState> data;

        public CarInfoDataSampleTestData()
        {
            data = new List<IState>
            {
                CreateCarInfoTestData(0),
                CreateCarInfoTestData(0, userId: 1),
                CreateCarInfoTestData(0, userId: 2, userName: "Dennis")
            };
        }

        private IState CreateCarInfoTestData(
            int carIdx,
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
            bool isSpectator = false
            )
        {
            List<Car> cars = new List<Car>();

            for (int i = 0; i < carIdx; i++)
            {
                cars.Add(new Car { CarIdx = i });
            }

            cars.Add(new Car
            {
                CarIdx = carIdx,
                CarNumber = carNumber,
                UserId = userId,
                UserName = userName,
                TeamId = teamId,
                TeamName = teamName,
                CarName = carScreenName,
                CarNameShort = carScreenNameShort,
                IRating = iRating,
                License = licString,
                IsSpectator = isSpectator,
            });

            return new State
            {
                SessionTime = sessionTime,
                Cars = cars.ToArray(),
                DriverCarIdx = driverCarIdx,
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return data.Select(m => new[] { m }).GetEnumerator(); // Create an object array with the data sample to be used by TestTheory data in tests as params
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}