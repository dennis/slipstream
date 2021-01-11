﻿using Slipstream.Shared.Events.IRacing;
using System;
using static Slipstream.Shared.Factories.IIRacingEventFactory;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class IRacingEventFactory : IIRacingEventFactory
    {
        public IRacingCarCompletedLap CreateIRacingCarCompletedLap(double sessionTime, long carIdx, double time, int lapsCompleted, float? fuelDiff, bool localUser)
        {
            return new IRacingCarCompletedLap
            {
                SessionTime = sessionTime,
                CarIdx = carIdx,
                Time = time,
                LapsCompleted = lapsCompleted,
                FuelDiff = fuelDiff,
                LocalUser = localUser,
            };
        }

        public IRacingCarInfo CreateIRacingCarInfo(
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

        public IRacingConnected CreateIRacingConnected()
        {
            return new IRacingConnected();
        }

        public IRacingCurrentSession CreateIRacingCurrentSession(IRacingCategoryEnum category, IRacingSessionTypeEnum sessionType, bool timeLimited, bool lapsLimited, int totalSessionLaps, double totalSessionTime)
        {
            string sessionTypeStr = sessionType switch
            {
                IRacingSessionTypeEnum.Practice => "Practice",
                IRacingSessionTypeEnum.OpenQualify => "OpenQualify",
                IRacingSessionTypeEnum.LoneQualify => "LoneQualify",
                IRacingSessionTypeEnum.OfflineTesting => "OfflineTesting",
                IRacingSessionTypeEnum.Race => "Race",
                IRacingSessionTypeEnum.Warmup => "Warmup",
                _ => throw new Exception($"Unexpected IRacingSessionTypeEnum '{sessionType}"),
            };

            string categoryStr = category switch
            {
                IRacingCategoryEnum.Road => "Road",
                IRacingCategoryEnum.Oval => "Oval",
                IRacingCategoryEnum.DirtOval => "DirtOval",
                IRacingCategoryEnum.DirtRoad => "DirtRoad",
                _ => throw new Exception($"Unexpected IRacingCategoryEnum '{category}"),
            };

            return new IRacingCurrentSession
            {
                Category = categoryStr,
                SessionType = sessionTypeStr,
                TimeLimited = timeLimited,
                LapsLimited = lapsLimited,
                TotalSessionLaps = totalSessionLaps,
                TotalSessionTime = totalSessionTime,
            };
        }

        public IRacingDisconnected CreateIRacingDisconnected()
        {
            return new IRacingDisconnected();
        }

        public IRacingPitEnter CreateIRacingPitEnter(double sessionTime, long carIdx, bool localUser)
        {
            return new IRacingPitEnter
            {
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser
            };
        }

        public IRacingPitExit CreateIRacingPitExit(double sessionTime, long carIdx, bool localUser, double? duration)
        {
            return new IRacingPitExit
            {
                SessionTime = sessionTime,
                CarIdx = carIdx,
                LocalUser = localUser,
                Duration = duration
            };
        }

        public IRacingPitstopReport CreateIRacingPitstopReport(
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
            float fuelDiff,
            double duration)
        {
            return new IRacingPitstopReport
            {
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
                FuelDiff = fuelDiff,
                Duration = duration
            };
        }

        public IRacingRaceFlags CreateIRacingRaceFlags(
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

        public IRacingSessionState CreateIRacingSessionState(double sessionTime, IRacingSessionStateEnum state)
        {
            return new IRacingSessionState
            {
                SessionTime = sessionTime,
                State = state.ToString(),
            };
        }

        public IRacingTrackInfo CreateIRacingTrackInfo
        (
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
            double sessionTime,
            string skies,
            string surfaceTemp,
            string airTemp,
            string airPressure,
            string relativeHumidity,
            string fogLevel)
        {
            return new IRacingWeatherInfo
            {
                SessionTime = sessionTime,
                Skies = skies,
                SurfaceTemp = surfaceTemp,
                AirTemp = airTemp,
                AirPressure = airPressure,
                RelativeHumidity = relativeHumidity,
                FogLevel = fogLevel,
            };
        }

        public IRacingCommandSendCarInfo CreateIRacingCommandSendCarInfo()
        {
            return new IRacingCommandSendCarInfo();
        }

        public IRacingCommandSendTrackInfo CreateIRacingCommandSendTrackInfo()
        {
            return new IRacingCommandSendTrackInfo();
        }

        public IRacingCommandSendWeatherInfo CreateIRacingCommandSendWeatherInfo()
        {
            return new IRacingCommandSendWeatherInfo();
        }

        public IRacingCommandSendCurrentSession CreateIRacingCommandSendCurrentSession()
        {
            return new IRacingCommandSendCurrentSession();
        }

        public IRacingCommandSendSessionState CreateIRacingCommandSendSessionState()
        {
            return new IRacingCommandSendSessionState();
        }

        public IRacingCommandSendRaceFlags CreateIRacingCommandSendRaceFlags()
        {
            return new IRacingCommandSendRaceFlags();
        }

        public IRacingDriverIncident CreateIRacingDriverIncident(int totalIncidents, int incidentDelta)
        {
            return new IRacingDriverIncident
            {
                IncidentCount = totalIncidents,
                IncidentDelta = incidentDelta
            };
        }
    }
}
