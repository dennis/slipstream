using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;

using System;

using static Slipstream.Components.IRacing.IIRacingEventFactory;

#nullable enable

namespace Slipstream.Components.IRacing.EventFactory
{
    public class IRacingEventFactory : IIRacingEventFactory
    {
        public IRacingCompletedLap CreateIRacingCompletedLap(
            IEventEnvelope envelope,
            double sessionTime,
            long carIdx,
            double lapTime,
            bool estimatedLapTime,
            int lapsCompleted,
            float? fuelLeft,
            float? fuelDelta,
            bool localUser,
            bool bestLap
        )
        {
            return new IRacingCompletedLap
            {
                Envelope = envelope.Clone(),
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LapTime = lapTime,
                EstimatedLapTime = estimatedLapTime,
                LapsCompleted = lapsCompleted,
                FuelLeft = fuelLeft,
                FuelDelta = fuelDelta,
                LocalUser = localUser,
                BestLap = bestLap,
            };
        }

        public IRacingCarInfo CreateIRacingCarInfo(
            IEventEnvelope envelope,
            double sessionTime,
            long carIdx,
            string carNumber,
            long currentDriverUserID,
            string currentDriverName,
            long teamID,
            string teamName,
            string carName,
            string carNameShort,
            long currentDriverIRating,
            string currentDriverLicense,
            bool localUser,
            bool spectator)
        {
            return new IRacingCarInfo
            {
                Envelope = envelope.Clone(),
                SessionTime = sessionTime,
                CarIdx = carIdx,
                CarNumber = carNumber,
                CurrentDriverUserID = currentDriverUserID,
                CurrentDriverName = currentDriverName,
                TeamID = teamID,
                TeamName = teamName,
                CarName = carName,
                CarNameShort = carNameShort,
                CurrentDriverIRating = currentDriverIRating,
                CurrentDriverLicense = currentDriverLicense,
                LocalUser = localUser,
                Spectator = spectator,
            };
        }

        public IRacingConnected CreateIRacingConnected(IEventEnvelope envelope)
        {
            return new IRacingConnected
            {
                Envelope = envelope.Clone(),
            };
        }

        private static string SessionCategoryToString(IRacingCategoryEnum category)
        {
            return category switch
            {
                IRacingCategoryEnum.Road => "Road",
                IRacingCategoryEnum.Oval => "Oval",
                IRacingCategoryEnum.DirtOval => "DirtOval",
                IRacingCategoryEnum.DirtRoad => "DirtRoad",
                _ => throw new Exception($"Unexpected IRacingCategoryEnum '{category}"),
            };
        }

        private string SessionStateToString(IRacingSessionStateEnum state)
        {
            return state switch
            {
                IRacingSessionStateEnum.Checkered => "Checkered",
                IRacingSessionStateEnum.CoolDown => "CoolDown",
                IRacingSessionStateEnum.GetInCar => "GetInCar",
                IRacingSessionStateEnum.Invalid => "Invalid",
                IRacingSessionStateEnum.ParadeLaps => "ParadeLaps",
                IRacingSessionStateEnum.Racing => "Racing",
                IRacingSessionStateEnum.Warmup => "Warmup",
                _ => throw new Exception($"Unexpected IRacingSessionStateEnum '{state}"),
            };
        }

        public IRacingDisconnected CreateIRacingDisconnected(IEventEnvelope envelope)
        {
            return new IRacingDisconnected { Envelope = envelope.Clone() };
        }

        public IRacingPitEnter CreateIRacingPitEnter(IEventEnvelope envelope, double sessionTime, long carIdx, bool localUser)
        {
            return new IRacingPitEnter
            {
                Envelope = envelope.Clone(),
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser
            };
        }

        public IRacingPitExit CreateIRacingPitExit(IEventEnvelope envelope, double sessionTime, long carIdx, bool localUser, double? duration, float? fuelLeft)
        {
            return new IRacingPitExit
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser,
                Duration = duration,
                FuelLeft = fuelLeft
            };
        }

        public IRacingPitstopReport CreateIRacingPitstopReport(
            IEventEnvelope envelope,
            double sessionTime,
            long carIdx,
            uint tempLFL,
            uint tempLFM,
            uint tempLFR,
            uint tempRFL,
            uint tempRFM,
            uint tempRFR,
            uint tempLRL,
            uint tempLRM,
            uint tempLRR,
            uint tempRRL,
            uint tempRRM,
            uint tempRRR,
            uint wearLFL,
            uint wearLFM,
            uint wearLFR,
            uint wearRFL,
            uint wearRFM,
            uint wearRFR,
            uint wearLRL,
            uint wearLRM,
            uint wearLRR,
            uint wearRRL,
            uint wearRRM,
            uint wearRRR,
            long laps,
            float fuelDelta,
            double duration)
        {
            return new IRacingPitstopReport
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                CarIdx = carIdx,

                TempLFL = tempLFL,
                TempLFM = tempLFM,
                TempLFR = tempLFR,

                TempRFL = tempRFL,
                TempRFM = tempRFM,
                TempRFR = tempRFR,

                TempLRL = tempLRL,
                TempLRM = tempLRM,
                TempLRR = tempLRR,

                TempRRL = tempRRL,
                TempRRM = tempRRM,
                TempRRR = tempRRR,

                WearLFL = wearLFL,
                WearLFM = wearLFM,
                WearLFR = wearLFR,

                WearRFL = wearRFL,
                WearRFM = wearRFM,
                WearRFR = wearRFR,

                WearLRL = wearLRL,
                WearLRM = wearLRM,
                WearLRR = wearLRR,

                WearRRL = wearRRL,
                WearRRM = wearRRM,
                WearRRR = wearRRR,

                Laps = laps,
                FuelDelta = fuelDelta,
                Duration = duration
            };
        }

        public IRacingRaceFlags CreateIRacingRaceFlags(
            IEventEnvelope envelope,
            double sessionTime,
            bool black,
            bool blue,
            bool caution,
            bool cautionWaving,
            bool checkered,
            bool crossed,
            bool debris,
            bool disqualify,
            bool fiveToGo,
            bool furled,
            bool green,
            bool greenHeld,
            bool oneLapToGreen,
            bool randomWaving,
            bool red,
            bool repair,
            bool servicible,
            bool startGo,
            bool startHidden,
            bool startReady,
            bool startSet,
            bool tenToGo,
            bool white,
            bool yellow,
            bool yellowWaving
        )
        {
            return new IRacingRaceFlags
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                Black = black,
                Blue = blue,
                Caution = caution,
                CautionWaving = cautionWaving,
                Checkered = checkered,
                Crossed = crossed,
                Debris = debris,
                Disqualify = disqualify,
                FiveToGo = fiveToGo,
                Furled = furled,
                Green = green,
                GreenHeld = greenHeld,
                OneLapToGreen = oneLapToGreen,
                RandomWaving = randomWaving,
                Red = red,
                Repair = repair,
                Servicible = servicible,
                StartGo = startGo,
                StartHidden = startHidden,
                StartReady = startReady,
                StartSet = startSet,
                TenToGo = tenToGo,
                White = white,
                Yellow = yellow,
                YellowWaving = yellowWaving,
            };
        }

        public IRacingTrackInfo CreateIRacingTrackInfo
        (
            IEventEnvelope envelope,
            long trackId,
            string trackLength,
            string trackDisplayName,
            string trackCity,
            string trackCountry,
            string trackDisplayShortName,
            string trackConfigName,
            string trackType)
        {
            return new IRacingTrackInfo
            {
                Envelope = envelope,
                TrackId = trackId,
                TrackLength = trackLength,
                TrackDisplayName = trackDisplayName,
                TrackCity = trackCity,
                TrackCountry = trackCountry,
                TrackDisplayShortName = trackDisplayShortName,
                TrackConfigName = trackConfigName,
                TrackType = trackType,
            };
        }

        public IRacingWeatherInfo CreateIRacingWeatherInfo(
            IEventEnvelope envelope,
            double sessionTime,
            Skies skies,
            float surfaceTemp,
            float airTemp,
            float airPressure,
            float relativeHumidity,
            float fogLevel)
        {
            return new IRacingWeatherInfo
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                Skies = skies.ToString(),
                SurfaceTemp = surfaceTemp,
                AirTemp = airTemp,
                AirPressure = airPressure,
                RelativeHumidity = relativeHumidity,
                FogLevel = fogLevel,
            };
        }

        public IRacingCommandSendCarInfo CreateIRacingCommandSendCarInfo(IEventEnvelope envelope)
        {
            return new IRacingCommandSendCarInfo { Envelope = envelope };
        }

        public IRacingCommandSendTrackInfo CreateIRacingCommandSendTrackInfo(IEventEnvelope envelope)
        {
            return new IRacingCommandSendTrackInfo { Envelope = envelope };
        }

        public IRacingCommandSendWeatherInfo CreateIRacingCommandSendWeatherInfo(IEventEnvelope envelope)
        {
            return new IRacingCommandSendWeatherInfo { Envelope = envelope };
        }

        public IRacingCommandSendSessionState CreateIRacingCommandSendSessionState(IEventEnvelope envelope)
        {
            return new IRacingCommandSendSessionState { Envelope = envelope };
        }

        public IRacingCommandSendRaceFlags CreateIRacingCommandSendRaceFlags(IEventEnvelope envelope)
        {
            return new IRacingCommandSendRaceFlags { Envelope = envelope };
        }

        public IRacingDriverIncident CreateIRacingDriverIncident(
            IEventEnvelope envelope,
            int driverIncidents,
            int driverIncidentsDelta,
            int teamIncidents,
            int teamIncidentsDelta,
            int myIncidents,
            int myIncidentsDelta)
        {
            return new IRacingDriverIncident
            {
                Envelope = envelope,
                DriverIncidentCount = driverIncidents,
                DriverIncidentDelta = driverIncidentsDelta,
                TeamIncidentCount = teamIncidents,
                TeamIncidentDelta = teamIncidentsDelta,
                MyIncidentCount = myIncidents,
                MyIncidentDelta = myIncidentsDelta,
            };
        }

        public IRacingPractice CreateIRacingPractice(
            IEventEnvelope envelope,
            double sessionTime,
            bool lapsLimited,
            bool timeLimited,
            double totalSessionTime,
            int totalSessionLaps,
            IRacingSessionStateEnum state,
            IRacingCategoryEnum category)
        {
            return new IRacingPractice
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                LapsLimited = lapsLimited,
                TimeLimited = timeLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
                State = SessionStateToString(state),
                Category = SessionCategoryToString(category)
            };
        }

        public IRacingQualify CreateIRacingQualify(
            IEventEnvelope envelope,
            double sessionTime,
            bool lapsLimited,
            bool timeLimited,
            double totalSessionTime,
            int totalSessionLaps,
            IRacingSessionStateEnum state,
            IRacingCategoryEnum category,
            bool openQualify)
        {
            return new IRacingQualify
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                LapsLimited = lapsLimited,
                TimeLimited = timeLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
                State = SessionStateToString(state),
                Category = SessionCategoryToString(category),
                OpenQualify = openQualify,
            };
        }

        public IRacingRace CreateIRacingRace(
            IEventEnvelope envelope,
            double sessionTime,
            bool lapsLimited,
            bool timeLimited,
            double totalSessionTime,
            int totalSessionLaps,
            IRacingSessionStateEnum state,
            IRacingCategoryEnum category)
        {
            return new IRacingRace
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                LapsLimited = lapsLimited,
                TimeLimited = timeLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
                State = SessionStateToString(state),
                Category = SessionCategoryToString(category)
            };
        }

        public IRacingTesting CreateIRacingTesting(
            IEventEnvelope envelope,
            double sessionTime,
            bool lapsLimited,
            bool timeLimited,
            double totalSessionTime,
            int totalSessionLaps,
            IRacingSessionStateEnum state,
            IRacingCategoryEnum category)
        {
            return new IRacingTesting
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                LapsLimited = lapsLimited,
                TimeLimited = timeLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
                State = SessionStateToString(state),
                Category = SessionCategoryToString(category)
            };
        }

        public IRacingWarmup CreateIRacingWarmup(
            IEventEnvelope envelope,
            double sessionTime,
            bool lapsLimited,
            bool timeLimited,
            double totalSessionTime,
            int totalSessionLaps,
            IRacingSessionStateEnum state,
            IRacingCategoryEnum category)
        {
            return new IRacingWarmup
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                LapsLimited = lapsLimited,
                TimeLimited = timeLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
                State = SessionStateToString(state),
                Category = SessionCategoryToString(category)
            };
        }

        public IRacingCarPosition CreateIRacingCarPosition(
            IEventEnvelope envelope,
            double sessionTime,
            int carIdx,
            bool localUser,
            int positionInClass,
            int positionInRace)
        {
            return new IRacingCarPosition
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser,
                PositionInClass = positionInClass,
                PositionInRace = positionInRace
            };
        }

        public IRacingRaw CreateIRacingRaw(IEventEnvelope envelope, IState state)
        {
            return new IRacingRaw
            {
                Envelope = envelope,
                CurrentState = state
            };
        }

        public IRacingTowed CreateIRacingTowed(IEventEnvelope envelope, double sessionTime, float remainingTowTime)
        {
            return new IRacingTowed
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                RemainingTowTime = remainingTowTime,
            };
        }

        public IRacingTrackPosition CreateIRacingTrackPosition(
            IEventEnvelope envelope,
            double sessionTime,
            long carIdx,
            bool localUser,
            int currentPositionInRace,
            int currentPositionInClass,
            int previousPositionInRace,
            int previousPositionInClass,
            int[] newCarsAhead,
            int[] newCarsBehind)
        {
            return new IRacingTrackPosition
            {
                Envelope = envelope,
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser,
                CurrentPositionInRace = currentPositionInRace,
                CurrentPositionInClass = currentPositionInClass,
                PreviousPositionInRace = previousPositionInRace,
                PreviousPositionInClass = previousPositionInClass,
                NewCarsAhead = newCarsAhead,
                NewCarsBehind = newCarsBehind,
            };
        }

        public IRacingCommandPitChangeLeftFrontTyre CreateIRacingCommandPitChangeLeftFrontTyre(IEventEnvelope envelope, int kpa)
        {
            return new IRacingCommandPitChangeLeftFrontTyre
            {
                Envelope = envelope,
                Kpa = kpa
            };
        }

        public IRacingCommandPitChangeRightFrontTyre CreateIRacingCommandPitChangeRightFrontTyre(IEventEnvelope envelope, int kpa)
        {
            return new IRacingCommandPitChangeRightFrontTyre
            {
                Envelope = envelope,
                Kpa = kpa
            };
        }

        public IRacingCommandPitChangeLeftRearTyre CreateIRacingCommandPitChangeLeftRearTyre(IEventEnvelope envelope, int kpa)
        {
            return new IRacingCommandPitChangeLeftRearTyre
            {
                Envelope = envelope,
                Kpa = kpa
            };
        }

        public IRacingCommandPitChangeRightRearTyre CreateIRacingCommandPitChangeRightRearTyre(IEventEnvelope envelope, int kpa)
        {
            return new IRacingCommandPitChangeRightRearTyre
            {
                Envelope = envelope,
                Kpa = kpa
            };
        }

        public IRacingCommandPitClearAll CreateIRacingCommandPitClearAll(IEventEnvelope envelope)
        {
            return new IRacingCommandPitClearAll { Envelope = envelope };
        }

        public IRacingCommandPitClearTyresChange CreateIRacingCommandPitClearTyresChange(IEventEnvelope envelope)
        {
            return new IRacingCommandPitClearTyresChange { Envelope = envelope };
        }

        public IRacingCommandPitRequestFastRepair CreateIRacingCommandPitRequestFastRepair(IEventEnvelope envelope)
        {
            return new IRacingCommandPitRequestFastRepair { Envelope = envelope };
        }

        public IRacingCommandPitAddFuel CreateIRacingCommandPitAddFuel(IEventEnvelope envelope, int addLiters)
        {
            return new IRacingCommandPitAddFuel
            {
                Envelope = envelope,
                AddLiters = addLiters
            };
        }

        public IRacingCommandPitCleanWindshield CreateIRacingCommandPitCleanWindshield(IEventEnvelope envelope)
        {
            return new IRacingCommandPitCleanWindshield { Envelope = envelope };
        }
    }
}