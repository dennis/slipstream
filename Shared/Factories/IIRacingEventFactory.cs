using Slipstream.Shared.Events.IRacing;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IIRacingEventFactory
    {
        public enum IRacingSessionTypeEnum
        {
            Practice, OpenQualify, LoneQualify, OfflineTesting, Race, Warmup
        }

        public enum IRacingSessionStateEnum
        {
            Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup
        }

        public enum IRacingCategoryEnum
        {
            Road, Oval, DirtOval, DirtRoad
        }

        IRacingCarCompletedLap CreateIRacingCarCompletedLap(double sessionTime, long carIdx, double time, int lapsCompleted, float? fuelDiff, bool localUser);

        IRacingCarInfo CreateIRacingCarInfo(
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
            bool spectator);

        IRacingConnected CreateIRacingConnected();

        IRacingDisconnected CreateIRacingDisconnected();

        IRacingPitEnter CreateIRacingPitEnter(double sessionTime, long carIdx, bool localUser);

        IRacingPitExit CreateIRacingPitExit(double sessionTime, long carIdx, bool localUser, double? duration);

        IRacingPitstopReport CreateIRacingPitstopReport(
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
            double duration);

        IRacingRaceFlags CreateIRacingRaceFlags(
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
        );

        IRacingTrackInfo CreateIRacingTrackInfo
        (
            long trackId,
            string trackLength,
            string trackDisplayName,
            string trackCity,
            string trackCountry,
            string trackDisplayShortName,
            string trackConfigName,
            string trackType);

        IRacingWeatherInfo CreateIRacingWeatherInfo(
            double sessionTime,
            string skies,
            string surfaceTemp,
            string airTemp,
            string airPressure,
            string relativeHumidity,
            string fogLevel);

        IRacingCommandSendCarInfo CreateIRacingCommandSendCarInfo();

        IRacingCommandSendTrackInfo CreateIRacingCommandSendTrackInfo();

        IRacingCommandSendWeatherInfo CreateIRacingCommandSendWeatherInfo();

        IRacingCommandSendSessionState CreateIRacingCommandSendSessionState();

        IRacingCommandSendRaceFlags CreateIRacingCommandSendRaceFlags();

        IRacingDriverIncident CreateIRacingDriverIncident(int totalIncidents, int incidentDelta);

        IRacingPractice CreateIRacingPractice(double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingQualify CreateIRacingQualify(double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingRace CreateIRacingRace(double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingTesting CreateIRacingTesting(double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingWarmup CreateIRacingWarmup(double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);
    }
}