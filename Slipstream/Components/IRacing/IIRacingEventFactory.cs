using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing
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

        public enum Skies
        {
            Clear = 0, PartlyCloudy = 1, MostlyCloudy = 2, Overcast = 3
        }

        public enum CarLocation
        {
            NotInWorld = -1,
            OffTrack,
            InPitStall,
            AproachingPits,
            OnTrack
        }

        IRacingCompletedLap CreateIRacingCompletedLap(
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
        );

        IRacingTowed CreateIRacingTowed(IEventEnvelope envelope, double sessionTime, float remainingTowTime);

        IRacingCarInfo CreateIRacingCarInfo(
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
            bool spectator);

        IRacingConnected CreateIRacingConnected(IEventEnvelope envelope);

        IRacingDisconnected CreateIRacingDisconnected(IEventEnvelope envelope);

        IRacingPitEnter CreateIRacingPitEnter(IEventEnvelope envelope, double sessionTime, long carIdx, bool localUser);

        IRacingPitExit CreateIRacingPitExit(IEventEnvelope envelope, double sessionTime, long carIdx, bool localUser, double? duration, float? fuelLeft);

        IRacingPitstopReport CreateIRacingPitstopReport(
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
            double duration);

        IRacingRaceFlags CreateIRacingRaceFlags(
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
        );

        IRacingTrackInfo CreateIRacingTrackInfo(
            IEventEnvelope envelope,
            long trackId,
            string trackLength,
            string trackDisplayName,
            string trackCity,
            string trackCountry,
            string trackDisplayShortName,
            string trackConfigName,
            string trackType);

        IRacingWeatherInfo CreateIRacingWeatherInfo(
            IEventEnvelope envelope,
            double sessionTime,
            Skies skies,
            float surfaceTemp,
            float airTemp,
            float airPressure,
            float relativeHumidity,
            float fogLevel);

        IRacingCommandSendCarInfo CreateIRacingCommandSendCarInfo(IEventEnvelope envelope);

        IRacingCommandSendTrackInfo CreateIRacingCommandSendTrackInfo(IEventEnvelope envelope);

        IRacingCommandSendWeatherInfo CreateIRacingCommandSendWeatherInfo(IEventEnvelope envelope);

        IRacingCommandSendSessionState CreateIRacingCommandSendSessionState(IEventEnvelope envelope);

        IRacingCommandSendRaceFlags CreateIRacingCommandSendRaceFlags(IEventEnvelope envelope);

        IRacingDriverIncident CreateIRacingDriverIncident(IEventEnvelope envelope, int driverIncidents, int driverIncidentsDelta, int teamIncidents, int teamIncidentsDelta, int myIncidents, int myIncidentsDelta);

        IRacingPractice CreateIRacingPractice(IEventEnvelope envelope, double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingQualify CreateIRacingQualify(IEventEnvelope envelope, double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category, bool openQualify);

        IRacingRace CreateIRacingRace(IEventEnvelope envelope, double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingTesting CreateIRacingTesting(IEventEnvelope envelope, double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingWarmup CreateIRacingWarmup(IEventEnvelope envelope, double sessionTime, bool lapsLimited, bool timeLimited, double totalSessionTime, int totalSessionLaps, IRacingSessionStateEnum state, IRacingCategoryEnum category);

        IRacingCarPosition CreateIRacingCarPosition(IEventEnvelope envelope, double sessionTime, int carIdx, bool localUser, int positionInClass, int positionInRace);

        IRacingRaw CreateIRacingRaw(IEventEnvelope envelope, IState state);

        IRacingTrackPosition CreateIRacingTrackPosition(IEventEnvelope envelope, double sessionTime, long carIdx, bool localUser, int currentPositionInRace, int currentPositionInClass, int previousPositionInRace, int previousPositionInClass, int[] newCarsAhead, int[] newCarsBehind);

        IRacingCommandPitChangeLeftFrontTyre CreateIRacingCommandPitChangeLeftFrontTyre(IEventEnvelope envelope, int kpa);

        IRacingCommandPitChangeRightFrontTyre CreateIRacingCommandPitChangeRightFrontTyre(IEventEnvelope envelope, int kpa);

        IRacingCommandPitChangeLeftRearTyre CreateIRacingCommandPitChangeLeftRearTyre(IEventEnvelope envelope, int kpa);

        IRacingCommandPitChangeRightRearTyre CreateIRacingCommandPitChangeRightRearTyre(IEventEnvelope envelope, int kpa);

        IRacingCommandPitClearAll CreateIRacingCommandPitClearAll(IEventEnvelope envelope);

        IRacingCommandPitClearTyresChange CreateIRacingCommandPitClearTyresChange(IEventEnvelope envelope);

        IRacingCommandPitRequestFastRepair CreateIRacingCommandPitRequestFastRepair(IEventEnvelope envelope);

        IRacingCommandPitAddFuel CreateIRacingCommandPitAddFuel(IEventEnvelope envelope, int addLiters);

        IRacingCommandPitCleanWindshield CreateIRacingCommandPitCleanWindshield(IEventEnvelope envelope);

        IRacingTime CreateIRacingTime(IEventEnvelope envelope, double sessionTime, double sessionTimeRemaining);
    }
}