using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.Plugins.GameState;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.UnitTests.TestData
{
    public class GameStateBuilder
    {
        public List<IState> States { get; }
        private readonly List<Car> Cars = new List<Car>();
        private readonly State State;

        public GameStateBuilder()
        {
            States = new List<IState>();
            State = new State
            {
                SessionTime = 0,
                SessionNum = 0,
                Cars = Cars.ToArray(),
                TrackLength = "3.34 km",
                TrackDisplayName = "Test Track",
                TrackCity = "Test City",
                TrackCountry = "Test Contry",
                TrackDisplayShortName = "Test",
                TrackConfigName = "TrackConfig",
                TrackType = "Race",
                Skies = Skies.Clear,
                TrackTempCrew = 25.1f,
                AirTemp = 20.3f,
                AirPressure = 34,
                DriverCarIdx = 0,
                RaceCategory = IIRacingEventFactory.IRacingCategoryEnum.Road,
                SessionFlags = SessionFlags.None,
                SessionState = IIRacingEventFactory.IRacingSessionStateEnum.Racing,
                SessionType = IIRacingEventFactory.IRacingSessionTypeEnum.Race,
                Sessions = new Session[]
                {
                    new Session()
                    {
                        LapsLimited = true,
                        TimeLimited = false,
                    }
                },
            };
        }

        public GameStateBuilder Set(Action<State> a)
        {
            a(State);

            return this;
        }

        public class CarBuilder
        {
            private readonly GameStateBuilder Builder;
            private readonly int CarIdx;
            private Car Car { get => Builder.Cars[CarIdx]; }

            public CarBuilder(GameStateBuilder b, int carIdx)
            {
                Builder = b;
                CarIdx = carIdx;

                Debug.Assert(b.Cars.Count >= carIdx);
                if (b.Cars.Count == carIdx)
                {
                    b.Cars.Add(new Car { CarIdx = carIdx });
                }
            }

            public CarBuilder LapsCompleted(int lap)
            {
                Set(a => a.LapsCompleted = lap);

                return this;
            }

            public CarBuilder ExitingPits()
            {
                Set(a => a.Location = IIRacingEventFactory.CarLocation.AproachingPits);

                return this;
            }

            public CarBuilder InPits()
            {
                Set(car => car.Location = IIRacingEventFactory.CarLocation.InPitStall);

                return this;
            }

            public CarBuilder EntersGame()
            {
                Set(car =>
                {
                    car.Location = IIRacingEventFactory.CarLocation.NotInWorld;
                    car.LapsCompleted = -1;
                });
                return this;
            }

            public CarBuilder Set(Action<Car> a)
            {
                a.Invoke(Car);
                Builder.RefreshCars();

                return this;
            }

            public CarBuilder AtLap(int v)
            {
                Set(a => a.LapsCompleted = v);

                return this;
            }

            public CarBuilder OnTrack()
            {
                Set(a => a.Location = CarLocation.OnTrack);

                return this;
            }

            public CarBuilder NotInWorld()
            {
                Set(a => a.Location = CarLocation.NotInWorld);

                return this;
            }

            public CarBuilder LastLapTime(float lapTime)
            {
                Set(a => a.LastLapTime = lapTime);

                return this;
            }

            public CarBuilder BestLapNum(int v)
            {
                Set(a => a.BestLapNum = v);

                return this;
            }

            public void Commit()
            {
                Builder.Commit();
            }

            public GameStateBuilder CarDone()
            {
                return Builder;
            }
        }

        public GameStateBuilder AtSessionTime(float sessionTime)
        {
            State.SessionTime = sessionTime;

            return this;
        }

        public GameStateBuilder InSessionNum(int sessionNum)
        {
            State.SessionNum = sessionNum;

            return this;
        }

        private void RefreshCars()
        {
            State.Cars = Cars.ToArray();
        }

        public CarBuilder Car(int carIdx)
        {
            return new CarBuilder(this, carIdx);
        }

        public GameStateBuilder FuelLevel(float v)
        {
            State.FuelLevel = v;

            return this;
        }

        public void Commit()
        {
            States.Add(State.Clone());
        }
    }
}