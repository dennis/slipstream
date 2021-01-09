using Slipstream.Shared.Events.Audio;
using Slipstream.Shared.Events.FileMonitor;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Events.IRacing;
using Slipstream.Shared.Events.Twitch;
using Slipstream.Shared.Events.UI;

#nullable enable

namespace Slipstream.Shared
{
    public interface IEventFactory
    {
        public enum PluginStatusEnum
        {
            Registered, Unregistered
        }

        public enum IRacingSessionTypeEnum
        {
            Practice, OpenQualify, LoneQualify, OfflineTesting, Race, Warmup
        }

        public enum IRacingSessionStateEnum
        {
            Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup
        }

        AudioCommandPlay CreateAudioCommandPlay(string filename, float? volume);
        AudioCommandSay CreateAudioCommandSay(string message, float? volume);

        FileMonitorFileChanged CreateFileMonitorFileChanged(string filePath);
        FileMonitorFileCreated CreateFileMonitorFileCreated(string path);
        FileMonitorFileDeleted CreateFileMonitorFileDeleted(string filePath);
        FileMonitorFileRenamed CreateFileMonitorFileRenamed(string filePath, string oldFilePath);

        InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName);
        InternalCommandPluginStates CreateInternalCommandPluginStates();
        InternalCommandPluginUnregister CreateInternalCommandPluginUnregister(string pluginId);
        InternalPluginState CreateInternalPluginState(string pluginId, string pluginName, string displayName, PluginStatusEnum pluginStatus);
        InternalCommandReconfigure CreateInternalCommandReconfigure();
        InternalCommandDeduplicateEvents CreateInternalCommandDeduplicateEvents(IEvent[] events);

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
            bool localUser,
            long spectator
        );
        IRacingConnected CreateIRacingConnected();
        IRacingCurrentSession CreateIRacingCurrentSession(IRacingSessionTypeEnum sessionType, bool timeLimited, bool lapsLimited, int totalSessionLaps, double totalSessionTime);
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
            double duration
        );

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

        IRacingSessionState CreateIRacingSessionState(double sessionTime, IRacingSessionStateEnum state);
        IRacingTrackInfo CreateIRacingTrackInfo(
            long trackId,
            string trackLength,
            string trackDisplayName,
            string trackCity,
            string trackCountry,
            string trackDisplayShortName,
            string trackConfigName,
            string trackType
        );
        IRacingWeatherInfo CreateIRacingWeatherInfo(
            double sessionTime,
            string skies,
            string surfaceTemp,
            string airTemp,
            string airPressure,
            string relativeHumidity,
            string fogLevel
        );
        IRacingCommandSendCarInfo CreateIRacingCommandSendCarInfo();
        IRacingCommandSendTrackInfo CreateIRacingCommandSendTrackInfo();
        IRacingCommandSendWeatherInfo CreateIRacingCommandSendWeatherInfo();
        IRacingCommandSendCurrentSession CreateIRacingCommandSendCurrentSession();
        IRacingCommandSendSessionState CreateIRacingCommandSendSessionState();
        IRacingCommandSendRaceFlags CreateIRacingCommandSendRaceFlags();
        FileMonitorCommandScan CreateFileMonitorCommandScan();

        TwitchCommandSendMessage CreateTwitchCommandSendMessage(string message);
        TwitchConnected CreateTwitchConnected();
        IRacingDriverIncident CreateIRacingDriverIncident(int totalIncidents, int incidentDelta);
        TwitchDisconnected CreateTwitchDisconnected();
        TwitchReceivedMessage CreateTwitchReceivedMessage(string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster);
        TwitchReceivedWhisper CreateTwitchReceivedWhisper(string from, string message);
        TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string to, string message);

        UICommandWriteToConsole CreateUICommandWriteToConsole(string message);
        UICommandCreateButton CreateUICommandCreateButton(string text);
        UICommandDeleteButton CreateUICommandDeleteButton(string text);
        UIButtonTriggered CreateUIButtonTriggered(string text);
    }
}
