using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class DebugOutputPlugin : BasePlugin
    {
        public DebugOutputPlugin(string id) : base(id, "DebugOutputPlugin", "DebugOutputPlugin", "Core")
        {
            EventHandler.OnDefault += (s, e) => EventHandler_OnDefault(e.Event);
            EventHandler.OnInternalPluginState += (s, e) => EventHandler_OnInternalPluginState(e.Event);
            EventHandler.OnInternalCommandPluginRegister += (s, e) => EventHandler_OnInternalPluginRegister(e.Event);
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => EventHandler_OnInternalPluginUnregister(e.Event);
            EventHandler.OnInternalCommandPluginEnable += (s, e) => EventHandler_OnInternalPluginEnable(e.Event);
            EventHandler.OnInternalCommandPluginDisable += (s, e) => EventHandler_OnInternalPluginDisable(e.Event);
            EventHandler.OnUtilityCommandWriteToConsole += (s, e) => EventHandler_OnUtilityWriteToConsole(e.Event);
            EventHandler.OnUtilityCommandSay += (s, e) => EventHandler_OnUtilitySay(e.Event);
            EventHandler.OnUtilityCommandPlayAudio += (s, e) => EventHandler_OnUtilityPlayAudio(e.Event);
            EventHandler.OnIRacingTrackInfo += (s, e) => EventHandler_OnIRacingTrackInfo(e.Event);
            EventHandler.OnIRacingWeatherInfo += (s, e) => EventHandler_OnIRacingWeatherInfo(e.Event);
            EventHandler.OnIRacingCurrentSession += (s, e) => EventHandler_OnIRacingCurrentSession(e.Event);
            EventHandler.OnIRacingCarInfo += (s, e) => EventHandler_OnIRacingCarInfo(e.Event);
            EventHandler.OnIRacingRaceFlags += (s, e) => EventHandler_OnIRacingRaceFlags(e.Event);
            EventHandler.OnIRacingSessionState += (s, e) => EventHandler_OnIRacingSessionState(e.Event);
            EventHandler.OnIRacingCarCompletedLap += (s, e) => EventHandler_OnIRacingCarCompletedLap(e.Event);
            EventHandler.OnIRacingPitEnter += (s, e) => EventHandler_OnIRacingPitEnter(e.Event);
            EventHandler.OnIRacingPitExit += (s, e) => EventHandler_OnIRacingPitExit(e.Event);
            EventHandler.OnIRacingPitstopReport += (s, e) => EventHandler_OnIRacingPitstopReport(e.Event);
            EventHandler.OnTwitchConnected += (s, e) => EventHandler_OnTwitchConnected(e.Event);
            EventHandler.OnTwitchDisconnected += (s, e) => EventHandler_OnTwitchDisconnected(e.Event);
            EventHandler.OnTwitchReceivedCommand += (s, e) => EventHandler_OnTwitchReceivedCommand(e.Event);
            EventHandler.OnSettingAudioSettings += (s, e) => EventHandler_OnSettingAudioSettings(e.Event);
            EventHandler.OnSettingFileMonitorSettings += (s, e) => EventHandler_OnSettingFileMonitorSettings(e.Event);
            EventHandler.OnSettingTwitchSettings += (s, e) => EventHandler_OnSettingTwitchSettings(e.Event);
            EventHandler.OnTwitchCommandSendMessage += (s, e) => EventHandler_OnTwitchSendMessage(e.Event);
        }

        private void EventHandler_OnTwitchSendMessage(Shared.Events.Twitch.CommandTwitchSendMessage ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Message}");
        }

        private void EventHandler_OnSettingTwitchSettings(Shared.Events.Setting.TwitchSettings ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.TwitchUsername}");
        }

        private void EventHandler_OnSettingFileMonitorSettings(Shared.Events.Setting.FileMonitorSettings ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Paths}");
        }

        private void EventHandler_OnSettingAudioSettings(Shared.Events.Setting.AudioSettings ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Path}");
        }

        private void EventHandler_OnTwitchReceivedCommand(Shared.Events.Twitch.TwitchReceivedCommand ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.From} {ev.Message} isModerator={ev.Moderator}, isVip={ev.Vip}, isSubscriber={ev.Subscriber}, isBroadcaster={ev.Broadcaster}");
        }

        private void EventHandler_OnTwitchDisconnected(Shared.Events.Twitch.TwitchDisconnected ev)
        {
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnTwitchConnected(Shared.Events.Twitch.TwitchConnected ev)
        {
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnIRacingPitstopReport(Shared.Events.IRacing.IRacingPitstopReport ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, CarIdx={ev.CarIdx}, Laps={ev.Laps}, FuelDiff={ev.FuelDiff}, Duration={ev.Duration}");
            Debug.WriteLine($"  {ev} Temperatures:");
            Debug.WriteLine($"  {ev}   LF {ev.TempLFL}/{ev.TempLFM}/{ev.TempLFR}");
            Debug.WriteLine($"  {ev}   RF {ev.TempLRL}/{ev.TempLRM}/{ev.TempLRR}");
            Debug.WriteLine($"  {ev}   LR {ev.TempRFL}/{ev.TempRFM}/{ev.TempRFR}");
            Debug.WriteLine($"  {ev}   RR {ev.TempRRL}/{ev.TempRRM}/{ev.TempRRR}");
            Debug.WriteLine($"  {ev} Wear:");
            Debug.WriteLine($"  {ev}   LF {ev.WearLFL}/{ev.WearLFM}/{ev.WearLFR}");
            Debug.WriteLine($"  {ev}   RF {ev.WearLRL}/{ev.WearLRM}/{ev.WearLRR}");
            Debug.WriteLine($"  {ev}   LR {ev.WearRFL}/{ev.WearRFM}/{ev.WearRFR}");
            Debug.WriteLine($"  {ev}   RR {ev.WearRRL}/{ev.WearRRM}/{ev.WearRRR}");
        }

        private void EventHandler_OnIRacingPitExit(Shared.Events.IRacing.IRacingPitExit ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}, Duration={ev.Duration}");
        }

        private void EventHandler_OnIRacingPitEnter(Shared.Events.IRacing.IRacingPitEnter ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}");
        }

        private void EventHandler_OnIRacingCarCompletedLap(Shared.Events.IRacing.IRacingCarCompletedLap ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, LapsComplete={ev.LapsComplete}, Time={ev.Time}, CarIndx={ev.CarIdx}, Fuel={ev.FuelDiff}");
        }

        private void EventHandler_OnIRacingSessionState(Shared.Events.IRacing.IRacingSessionState ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, State={ev.State}");
        }

        private void EventHandler_OnIRacingRaceFlags(Shared.Events.IRacing.IRacingRaceFlags ev)
        {
            Debug.Write($"$$ {ev} SessionTime={ev.SessionTime} ");

            if (ev.Black)
                Debug.Write("Black ");
            if (ev.Blue)
                Debug.Write("Blue ");
            if (ev.Caution)
                Debug.Write("Caution ");
            if (ev.CautionWaving)
                Debug.Write("CautionWaving ");
            if (ev.Checkered)
                Debug.Write("Checkered ");
            if (ev.Crossed)
                Debug.Write("Crossed ");
            if (ev.Debris)
                Debug.Write("Debris ");
            if (ev.Disqualify)
                Debug.Write("Disqualify ");
            if (ev.FiveToGo)
                Debug.Write("FiveToGo ");
            if (ev.Furled)
                Debug.Write("Furled ");
            if (ev.Green)
                Debug.Write("Green ");
            if (ev.GreenHeld)
                Debug.Write("GreenHeld ");
            if (ev.RandomWaving)
                Debug.Write("RandomWaving ");
            if (ev.Red)
                Debug.Write("Red ");
            if (ev.Repair)
                Debug.Write("Repair ");
            if (ev.Servicible)
                Debug.Write("Servicible ");
            if (ev.StartGo)
                Debug.Write("StartGo ");
            if (ev.StartHidden)
                Debug.Write("StartHidden ");
            if (ev.StartReady)
                Debug.Write("StartReady ");
            if (ev.StartSet)
                Debug.Write("StartSet ");
            if (ev.TenToGo)
                Debug.Write("TenToGo "); 
            if (ev.White)
                Debug.Write("White ");
            if (ev.Yellow)
                Debug.Write("Yellow ");
            if (ev.YellowWaving)
                Debug.Write("YellowWaving ");

            Debug.WriteLine("");
        }

        private void EventHandler_OnIRacingCarInfo(Shared.Events.IRacing.IRacingCarInfo ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}, CarNumber={ev.CarNumber}, CarName={ev.CarName}, CarNameShort={ev.CarNameShort}, CurrentDriverName={ev.CurrentDriverName}, CurrentDriverUserID={ev.CurrentDriverUserID}, CurrentDriverIRating={ev.CurrentDriverIRating}, TeamID={ev.TeamID}, TeamName={ev.TeamName}, Spectator={ev.Spectator}");
        }

        private void EventHandler_OnIRacingCurrentSession(Shared.Events.IRacing.IRacingCurrentSession ev)
        {
            Debug.WriteLine($"$$ {ev} LapsLimited={ev.LapsLimited}, SessionLaps={ev.TotalSessionLaps}, SessionType={ev.SessionType}, TimeLimited={ev.TimeLimited}, SessionTime={ev.TotalSessionTime}");
        }

        private void EventHandler_OnIRacingWeatherInfo(Shared.Events.IRacing.IRacingWeatherInfo ev)
        {
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, AirTemp={ev.AirTemp}, SurfaceTemp={ev.SurfaceTemp}, FogLevel={ev.FogLevel}");
        }

        private void EventHandler_OnIRacingTrackInfo(Shared.Events.IRacing.IRacingTrackInfo ev)
        {
            Debug.WriteLine($"$$ {ev} TrackDisplayShortName={ev.TrackDisplayShortName}, TrackCity={ev.TrackCity}, TrackConfigName={ev.TrackConfigName}, TrackDisplayName={ev.TrackDisplayName}, TrackId={ev.TrackId}, TrackLength={ev.TrackLength}, TrackType={ev.TrackType}");
        }

        private void EventHandler_OnUtilityPlayAudio(Shared.Events.Utility.CommandPlayAudio ev)
        {
            Debug.WriteLine($"$$ {ev} message=\"{ev.Filename}\", volume={ev.Volume}");
        }

        private void EventHandler_OnInternalPluginDisable(Shared.Events.Internal.CommandPluginDisable ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginEnable(Shared.Events.Internal.CommandPluginEnable ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginUnregister(Shared.Events.Internal.CommandPluginUnregister ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginRegister(Shared.Events.Internal.CommandPluginRegister ev)
        {
            Debug.WriteLine($"$$ {ev} Id={ev.Id} PluginName={ev.PluginName}");
        }

        private void EventHandler_OnUtilitySay(Shared.Events.Utility.CommandSay ev)
        {
            Debug.WriteLine($"$$ {ev} message=\"{ev.Message}\" @ volume={ev.Volume}");
        }

        private void EventHandler_OnUtilityWriteToConsole(Shared.Events.Utility.CommandWriteToConsole ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Message}");
        }

        private void EventHandler_OnInternalPluginState(Shared.Events.Internal.PluginState ev)
        {
            Debug.WriteLine($"$$ {ev} {ev.Id} {ev.PluginName} {ev.PluginStatus}");
        }

        private void EventHandler_OnDefault(IEvent @event)
        {
            Debug.WriteLine($"$$ {@event}");
        }
    }
}
