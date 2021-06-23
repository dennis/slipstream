using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class TrackPositionTracker : IIRacingDataTracker
    {
        // Keep track of track positions is a bit complicated.
        //
        // The base idea is that we store an AbsolutePosition float, which is
        // <lapnumber>.<pct-of-completed-lap>. Which means that if you are halfway
        // through lap five, your position would be 5.5.
        //
        // By assigning an AbsolutePosition to all cars, we can simply sort the
        // list from low-to-high, to determine what position they got right now.
        //
        // IRacing provides Laps and LapsCompleted for giving a lap-count and LapDistPct
        // that represent where you are on the track on that given lap. So this can almost
        // be used directly. We'll create a number in the format of <Lap>.<LapDistPct>,
        // meaning that 4.32, means that we have completed 4 full laps, and is 32% into
        // the next one.
        //
        // However,  race starts complicates it a bit. When before green LapsCompleted is
        // -1 and Laps is 0. When we go green, both of them is increased by 1. So a starting
        // car would have AbsolutePosition = 0.9 - as we're about to cross start/finish line.
        //
        // So far, our AbsolutePosition works nicely.
        //
        // Crossing s/f line after green, and thereby starting your first lap, will NOT increase
        // LapsCompleted and Laps. So our car will now be at 0.1.. and cars that have not
        // cross s/f will have 0.9-something. Sorting these values will appear as if the cars
        // not crossed the s/f line is far ahead of the cars that did.
        //
        // To fix this, we try to detect the "PreLap". The lap we're on when we're green, before
        // crossing s/f line. If this PreLap is true, we will subtract one from the lap value,
        // so that cars that didnt cross s/f line, will be at -1.9, whilst cars that did will be at
        // 0.2. Now our sorting works again.
        //
        // Related to this, some drivers starts from the pits. These cars will have position 0.9x
        // so the cars on the track might seem behind the pitted cars. As starting from pits will
        // make IRacing delay when you can start - you will eventually get on track after all the
        // cars that started on the track. So to avoid a number of irrelevant overtakes, we'll
        // ignore these cars until they exit the pits.
        //
        // Towed cars: When a car is towed to pits, the position is immediately changed to the
        // position of of the pit stall. This gives some wierd effects.
        // Say car A is at 3.5, and car B is at 3.7. Car A crashes and is towed, which means that
        // the car will instantly be moved to 3.9 - overtaking car B. It will stay there for the
        // duration of the towing, so car B will regain its position. But we'll end up having two
        // overtakes, that isn't really worth reporting. So to get around this, I'm ignoring the
        // new position if we're being towed. So car A will seemingly still be at 3.5. Once the
        // car leaves the pits, it will then be zapped from 3.5 to 3.9.. It would be neat if we could
        // avoid this. But we don't know how long the towing will take.

        private class CarData
        {
            public float AbsolutePosition { get; set; }
            public float LastAbsolutePosition { get; set; } // DEBUG
            public int CurrentPositionInClass { get; set; }
            public int CurrentPositionInRace { get; set; }
            public int PreviousPositionInRace { get; set; }
            public int PreviousPositionInClass { get; set; }
            public int LastLap { get; set; }
            public float LastLapDistPct { get; set; }
            public bool PreLap { get; set; } = false;
            public IIRacingEventFactory.CarLocation LastLocation { get; set; }
            public bool IgnoredUntilOnTrack { get; set; } = false;
            public double NotInWorldSince { get; set; } = -1;
            public long CarClassId { get; set; }
        }

        private class PositionChange
        {
            public int CarIdx { get; set; }
            public int CurrentPositionInRace { get; set; }
            public int CurrentPositionInClass { get; set; }
            public int PreviousPositionInRace { get; set; }
            public int PreviousPositionInClass { get; set; }
        }

        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        private readonly Dictionary<int, CarData> LastCarData = new Dictionary<int, CarData>();
        private int SessionNum = 0;
        private double LastOverviewShownAt { get; set; }

        private IIRacingEventFactory.IRacingSessionStateEnum LastSessionState = IIRacingEventFactory.IRacingSessionStateEnum.Invalid;

        public TrackPositionTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            if (currentState.SessionNum != SessionNum)
            {
                LastCarData.Clear();
                SessionNum = currentState.SessionNum;
            }

            if (currentState.SessionType != IIRacingEventFactory.IRacingSessionTypeEnum.Race)
            {
                return;
            }

            if (LastSessionState != currentState.SessionState)
            {
                Debug($"{currentState.SessionType}: {LastSessionState} -> {currentState.SessionState}");

                // Populate our state
                if (LastCarData.Count == 0 && (currentState.SessionState == IIRacingEventFactory.IRacingSessionStateEnum.Racing || currentState.SessionState == IIRacingEventFactory.IRacingSessionStateEnum.Warmup))
                {
                    InitializeTrackerState(currentState);
                }

                LastSessionState = currentState.SessionState;
            }

            if (currentState.SessionState != IIRacingEventFactory.IRacingSessionStateEnum.Racing)
            {
                return;
            }

            bool someoneIsBlinking = UpdateCarPositions(currentState);

            // Don't calculate new positions if somebody is blinking.
            if (someoneIsBlinking)
                return;

            RecalculatePositions(currentState);
            PublishEvents(currentState, envelope);
            ShowOverview(currentState);
        }

        private bool UpdateCarPositions(GameState.IState currentState)
        {
            var someoneIsBlinking = false;

            // calculate positions
            foreach (var car in currentState.Cars)
            {
                if (car.UserId != -1 && !car.IsSpectator) // ignore pacecar and spectators
                    someoneIsBlinking = UpdateTrackerState(currentState, someoneIsBlinking, car);
            }

            return someoneIsBlinking;
        }

        private void ShowOverview(GameState.IState currentState)
        {
            if (currentState.SessionTime - LastOverviewShownAt > 60)
            {
                foreach (var orderedPosition in LastCarData
                    .OrderByDescending(x => x.Value.AbsolutePosition)
                    .ToList())
                {
                    Debug($"  p{orderedPosition.Value.CurrentPositionInRace}/{orderedPosition.Value.CurrentPositionInClass}  carIdx={orderedPosition.Key} {currentState.Cars[orderedPosition.Key].CarNumber} {currentState.Cars[orderedPosition.Key].UserName} {orderedPosition.Value.AbsolutePosition} {orderedPosition.Value.PreLap}/{orderedPosition.Value.LastLap} {orderedPosition.Value.LastLocation}");
                }

                LastOverviewShownAt = currentState.SessionTime;
            }
        }

        private void PublishEvents(GameState.IState currentState, IEventEnvelope envelope)
        {
            var time = currentState.SessionTime;
            var positionChanges = new List<PositionChange>();

            foreach (var orderedPosition in LastCarData)
            {
                var carIdx = orderedPosition.Key;
                var trackerCarState = orderedPosition.Value;

                if (trackerCarState.CurrentPositionInRace == trackerCarState.PreviousPositionInRace)
                {
                    // No change, all no event
                }
                else
                {
                    // We gained/lost one or more positions
                    positionChanges.Add(
                        new PositionChange
                        {
                            CarIdx = carIdx,
                            CurrentPositionInRace = trackerCarState.CurrentPositionInRace,
                            PreviousPositionInRace = trackerCarState.PreviousPositionInRace,
                            CurrentPositionInClass = trackerCarState.CurrentPositionInClass,
                            PreviousPositionInClass = trackerCarState.PreviousPositionInClass,
                        });
                }
            }

            // We know the changes now, let's see if we can improve upon them
            foreach (var change in positionChanges)
            {
                if (change.PreviousPositionInRace == 0)
                {
                    // The car didnt have a position before, so no need to send out event
                    LastCarData[change.CarIdx].PreviousPositionInRace = LastCarData[change.CarIdx].CurrentPositionInRace;
                }
                else
                {
                    var newCarsAhead = new List<int>();
                    var newCarsBehind = new List<int>();

                    foreach (var otherCar in positionChanges.Where(a => a.CarIdx != change.CarIdx && a.CurrentPositionInRace != a.PreviousPositionInRace))
                    {
                        if (change.CurrentPositionInRace < otherCar.CurrentPositionInRace && change.PreviousPositionInRace >= otherCar.CurrentPositionInRace)
                        {
                            newCarsBehind.Add(otherCar.CarIdx);
                        }
                        else if (change.CurrentPositionInRace > otherCar.CurrentPositionInRace && change.PreviousPositionInRace <= otherCar.CurrentPositionInRace)
                        {
                            newCarsAhead.Add(otherCar.CarIdx);
                        }
                    }

                    EventBus.PublishEvent(
                        EventFactory.CreateIRacingTrackPosition(
                            envelope: envelope,
                            sessionTime: time,
                            carIdx: change.CarIdx,
                            localUser: currentState.DriverCarIdx == change.CarIdx,
                            currentPositionInRace: change.CurrentPositionInRace,
                            currentPositionInClass: change.CurrentPositionInClass,
                            previousPositionInRace: change.PreviousPositionInRace,
                            previousPositionInClass: change.PreviousPositionInClass,
                            newCarsAhead: newCarsAhead.ToArray(),
                            newCarsBehind: newCarsBehind.ToArray()
                        )
                    );
                }

                LastCarData[change.CarIdx].PreviousPositionInRace = LastCarData[change.CarIdx].CurrentPositionInRace;
            }
        }

        private void RecalculatePositions(GameState.IState currentState)
        {
            int nextPositionInRace = 1;
            var nextPositionInClass = new Dictionary<long, int>();
            foreach (var orderedPosition in LastCarData
                .Where(x => !x.Value.IgnoredUntilOnTrack)
                .OrderByDescending(x => x.Value.AbsolutePosition)
                .ToList())
            {
                var carIdx = orderedPosition.Key;
                var trackerCarState = orderedPosition.Value;

                if (!nextPositionInClass.ContainsKey(trackerCarState.CarClassId))
                {
                    nextPositionInClass.Add(trackerCarState.CarClassId, 1);
                }
                else
                {
                    nextPositionInClass[trackerCarState.CarClassId]++;
                }

                if (nextPositionInRace != trackerCarState.CurrentPositionInRace)
                {
                    Debug($"### {currentState.SessionTime} caridx-{carIdx}: carNumber={currentState.Cars[carIdx].CarNumber} PreLap={trackerCarState.PreLap}, Ignored={trackerCarState.IgnoredUntilOnTrack}, absPos={trackerCarState.LastAbsolutePosition} -> {trackerCarState.AbsolutePosition}, LastLap={trackerCarState.LastLap}, Location={currentState.Cars[carIdx].Location}: PositionInRace={trackerCarState.CurrentPositionInRace}->{nextPositionInRace}, PositionInClass={trackerCarState.CurrentPositionInClass}->{nextPositionInClass[trackerCarState.CarClassId]} {currentState.Cars[orderedPosition.Key].UserName}");

                    trackerCarState.PreviousPositionInRace = trackerCarState.CurrentPositionInRace;
                    trackerCarState.PreviousPositionInClass = trackerCarState.CurrentPositionInClass;
                    trackerCarState.CurrentPositionInRace = nextPositionInRace;
                    trackerCarState.CurrentPositionInClass = nextPositionInClass[trackerCarState.CarClassId];
                }

                nextPositionInRace++;
            }
        }

        private bool UpdateTrackerState(GameState.IState currentState, bool someoneIsBlinking, GameState.Car car)
        {
            var carTrackerState = LastCarData[car.CarIdx];

            carTrackerState.LastAbsolutePosition = carTrackerState.AbsolutePosition;

            if (carTrackerState.IgnoredUntilOnTrack)
            {
                HandleIgnoredCar(car, carTrackerState);
            }
            else if (car.Location == IIRacingEventFactory.CarLocation.InPitStall && carTrackerState.LastLocation == IIRacingEventFactory.CarLocation.NotInWorld)
            {
                HandleTowedCar(car, carTrackerState);
            }
            else if (car.Location == IIRacingEventFactory.CarLocation.InPitStall && carTrackerState.IgnoredUntilOnTrack)
            {
                // Still being towed / in pits
            }
            else if (car.Location == IIRacingEventFactory.CarLocation.NotInWorld)
            {
                someoneIsBlinking = HandleCarNotInWorld(currentState, someoneIsBlinking, car, carTrackerState);
            }
            else
            {
                HandleRacingCar(currentState, car, carTrackerState);
            }

            return someoneIsBlinking;
        }

        private void HandleRacingCar(GameState.IState currentState, GameState.Car car, CarData carTrackerState)
        {
            // Car is racing

            var lapDistPct = car.LapDistPct;

            if (lapDistPct > 1)
            {
                Debug($"Fixing bad position for caridx-{car.CarIdx} {car.CarNumber}  - above 100% track completed!? {lapDistPct}");
                lapDistPct = 1;
            }

            if (carTrackerState.PreLap)
            {
                var diff = carTrackerState.LastLapDistPct - lapDistPct;

                if (diff > 0.9)
                {
                    Debug($"{currentState.SessionTime} caridx-{car.CarIdx} {car.CarNumber}: s/f crossed - diff {diff} - no more prelap");
                    carTrackerState.PreLap = false;
                }
            }

            /* Handle that IRacing will report:
             * lapDistPct 0,9996102 of lap 0
             * lapDistPct 0,9998848 of lap 1 <-- end of next lap!?
             * lapDistPct 0,0001598561 of lap 1
             */

            if (car.Laps > carTrackerState.LastLap && lapDistPct > 0.9f)
            {
                Debug($"Fixing bad position for caridx-{car.CarIdx} car.Laps={car.Laps}, carTrackerState.LastLap={carTrackerState.LastLap}, lapDistPct={lapDistPct}");
                lapDistPct = 0;
            }

            var laps = car.Laps;

            if (carTrackerState.PreLap)
            {
                laps--;
            }

            carTrackerState.AbsolutePosition = (float)(laps + lapDistPct); // <lap>.<lapdistpctpct>
            carTrackerState.LastLap = car.Laps;
            carTrackerState.LastLapDistPct = lapDistPct;
            carTrackerState.LastLocation = car.Location;
            carTrackerState.IgnoredUntilOnTrack = false;
            carTrackerState.NotInWorldSince = -1;
        }

        private bool HandleCarNotInWorld(GameState.IState currentState, bool someoneIsBlinking, GameState.Car car, CarData carTrackerState)
        {
            // This could be start of towing, or blinking.
            carTrackerState.LastLocation = car.Location;

            if (carTrackerState.NotInWorldSince == -1)
            {
                carTrackerState.NotInWorldSince = currentState.SessionTime;
            }
            else
            {
                var delta = currentState.SessionTime - carTrackerState.NotInWorldSince;

                if (delta < 1)
                {
                    someoneIsBlinking = true;
                }
            }

            return someoneIsBlinking;
        }

        private void HandleTowedCar(GameState.Car car, CarData carTrackerState)
        {
            Debug($"caridx = {car.CarIdx} {car.CarNumber} {car.UserName} is in pit stall after being notinworld. Towed or started from pits. Ignoring");

            carTrackerState.LastLocation = car.Location;
            carTrackerState.IgnoredUntilOnTrack = true;
            carTrackerState.NotInWorldSince = -1;
            // We dont update the position intentionally. We keep the old position and let other cars
            // overtake us. Once we're leaving the pits, we'll update the position.
        }

        private void HandleIgnoredCar(GameState.Car car, CarData carTrackerState)
        {
            if (car.Location == IIRacingEventFactory.CarLocation.OnTrack || car.Location == IIRacingEventFactory.CarLocation.OffTrack)
            {
                Debug($"caridx-{car.CarIdx} is back on track. Stop ignoring {car.UserName}");
                carTrackerState.IgnoredUntilOnTrack = false;
                carTrackerState.PreLap = false;
            }
        }

        private void InitializeTrackerState(GameState.IState currentState)
        {
            Debug("** POPULATING **");

            LastOverviewShownAt = currentState.SessionTime;

            foreach (var car in currentState.Cars)
            {
                if (car.UserId != -1 && !car.IsSpectator)
                {
                    var ignoredUntilOnTrack = car.Location == IIRacingEventFactory.CarLocation.InPitStall;
                    var preLap = car.Laps == 1 && LastSessionState != IIRacingEventFactory.IRacingSessionStateEnum.Invalid; // Initially lastsessionstate is invalid, so if we see it here, it mean that we're dropped directly into a race without any warmup/getincar/paradealps etc
                    var laps = car.Laps;
                    var lapDistPct = car.LapDistPct;

                    // Some tracks some of the cars starts in front of the s/f line (like Bathurst) and the
                    // rest behind the line. To properly be able to sort this, we'll pretend they are a
                    // lap down, so they dont get calculated as in front of everbody. We do this by setting
                    // preLap

                    if (lapDistPct > 0.5)
                    {
                        preLap = true;
                    }

                    Debug($"{currentState.SessionTime} Hello caridx-#{car.CarIdx} {car.CarNumber} {car.UserName} laps={car.Laps}, LapDistPct={car.LapDistPct}, PreLap={preLap}, Location={car.Location}, IgnoredUntilOnTrack={ignoredUntilOnTrack}");

                    if (preLap)
                    {
                        laps--;
                    }

                    LastCarData.Add(car.CarIdx, new CarData
                    {
                        AbsolutePosition = laps + car.LapDistPct,
                        CurrentPositionInClass = car.ClassPosition,
                        CurrentPositionInRace = car.Position,
                        LastLap = laps,
                        LastLapDistPct = car.LapDistPct,
                        LastLocation = car.Location,
                        PreLap = preLap,
                        IgnoredUntilOnTrack = ignoredUntilOnTrack,
                        CarClassId = car.CarClassId,
                    });
                }
                else
                {
                    Debug($"Ignoring {car.CarIdx} {car.UserName}");
                }
            }
        }

        private void Debug(string _)
        {
            //System.Diagnostics.Debug.WriteLine(str);
        }
    }
}