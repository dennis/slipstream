using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class DebugOutputPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "DebugOutputPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; internal set; }
        public string WorkerName => "Core";
        private IEventBusSubscription? EventBusSubscription;
        private readonly EventHandler EventHandler = new EventHandler();

        public DebugOutputPlugin(string id)
        {
            Id = id;

            EventHandler.OnDefault += EventHandler_OnDefault;
            EventHandler.OnInternalPluginStateChanged += EventHandler_OnInternalPluginStateChanged;
            EventHandler.OnInternalPluginRegister += EventHandler_OnInternalPluginRegister;
            EventHandler.OnInternalPluginUnregister += EventHandler_OnInternalPluginUnregister;
            EventHandler.OnInternalPluginEnable += EventHandler_OnInternalPluginEnable;
            EventHandler.OnInternalPluginDisable += EventHandler_OnInternalPluginDisable;
            EventHandler.OnUtilityWriteToConsole += EventHandler_OnUtilityWriteToConsole;
            EventHandler.OnUtilitySay += EventHandler_OnUtilitySay;
            EventHandler.OnUtilityPlayAudio += EventHandler_OnUtilityPlayAudio;
            EventHandler.OnIRacingTrackInfo += EventHandler_OnIRacingTrackInfo;
            EventHandler.OnIRacingWeatherInfo += EventHandler_OnIRacingWeatherInfo;
            EventHandler.OnIRacingCurrentSession += EventHandler_OnIRacingCurrentSession;
            EventHandler.OnIRacingCarInfo += EventHandler_OnIRacingCarInfo;
            EventHandler.OnIRacingRaceFlags += EventHandler_OnIRacingRaceFlags;
            EventHandler.OnIRacingSessionState += EventHandler_OnIRacingSessionState;
            EventHandler.OnIRacingCarCompletedLap += EventHandler_OnIRacingCarCompletedLap;
            EventHandler.OnIRacingPitEnter += EventHandler_OnIRacingPitEnter;
            EventHandler.OnIRacingPitExit += EventHandler_OnIRacingPitExit;
            EventHandler.OnIRacingPitstopReport += EventHandler_OnIRacingPitstopReport;
            EventHandler.OnTwitchConnected += EventHandler_OnTwitchConnected;
            EventHandler.OnTwitchDisconnected += EventHandler_OnTwitchDisconnected;
            EventHandler.OnTwitchReceivedCommand += EventHandler_OnTwitchReceivedCommand;
            EventHandler.OnSettingAudioSettings += EventHandler_OnSettingAudioSettings;
            EventHandler.OnSettingFileMonitorSettings += EventHandler_OnSettingFileMonitorSettings;
            EventHandler.OnSettingTwitchSettings += EventHandler_OnSettingTwitchSettings;
        }

        private void EventHandler_OnSettingTwitchSettings(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Setting.TwitchSettings> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnSettingFileMonitorSettings(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Setting.FileMonitorSettings> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnSettingAudioSettings(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Setting.AudioSettings> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnTwitchReceivedCommand(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Twitch.TwitchReceivedCommand> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.From} {ev.Message} isModerator={ev.Moderator}, isVip={ev.Vip}, isSubscriber={ev.Subscriber}, isBroadcaster={ev.Broadcaster}");
        }

        private void EventHandler_OnTwitchDisconnected(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Twitch.TwitchDisconnected> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnTwitchConnected(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Twitch.TwitchConnected> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev}");
        }

        private void EventHandler_OnIRacingPitstopReport(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingPitstopReport> e)
        {
            var ev = e.Event;
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

        private void EventHandler_OnIRacingPitExit(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingPitExit> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}, Duration={ev.Duration}");
        }

        private void EventHandler_OnIRacingPitEnter(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingPitEnter> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}");
        }

        private void EventHandler_OnIRacingCarCompletedLap(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingCarCompletedLap> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, LapsComplete={ev.LapsComplete}, Time={ev.Time}, CarIndx={ev.CarIdx}, Fuel={ev.FuelDiff}");
        }

        private void EventHandler_OnIRacingSessionState(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingSessionState> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, State={ev.State}");
        }

        private void EventHandler_OnIRacingRaceFlags(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingRaceFlags> e)
        {
            var ev = e.Event;
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

        private void EventHandler_OnIRacingCarInfo(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingCarInfo> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, LocalUser={ev.LocalUser}, CarIdx={ev.CarIdx}, CarNumber={ev.CarNumber}, CarName={ev.CarName}, CarNameShort={ev.CarNameShort}, CurrentDriverName={ev.CurrentDriverName}, CurrentDriverUserID={ev.CurrentDriverUserID}, CurrentDriverIRating={ev.CurrentDriverIRating}, TeamID={ev.TeamID}, TeamName={ev.TeamName}, Spectator={ev.Spectator}");
        }

        private void EventHandler_OnIRacingCurrentSession(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingCurrentSession> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} LapsLimited={ev.LapsLimited}, SessionLaps={ev.TotalSessionLaps}, SessionType={ev.SessionType}, TimeLimited={ev.TimeLimited}, SessionTime={ev.TotalSessionTime}");
        }

        private void EventHandler_OnIRacingWeatherInfo(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingWeatherInfo> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} SessionTime={ev.SessionTime}, AirTemp={ev.AirTemp}, SurfaceTemp={ev.SurfaceTemp}, FogLevel={ev.FogLevel}");
        }

        private void EventHandler_OnIRacingTrackInfo(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.IRacing.IRacingTrackInfo> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} TrackDisplayShortName={ev.TrackDisplayShortName}, TrackCity={ev.TrackCity}, TrackConfigName={ev.TrackConfigName}, TrackDisplayName={ev.TrackDisplayName}, TrackId={ev.TrackId}, TrackLength={ev.TrackLength}, TrackType={ev.TrackType}");
        }

        private void EventHandler_OnUtilityPlayAudio(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.PlayAudio> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} message=\"{ev.Filename}\", volume={ev.Volume}");
        }

        private void EventHandler_OnInternalPluginDisable(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginDisable> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginEnable(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginEnable> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginUnregister(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginUnregister> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id}");
        }

        private void EventHandler_OnInternalPluginRegister(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginRegister> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id} pluginname={ev.PluginName}, enabled={ev.Enabled}");
        }

        private void EventHandler_OnUtilitySay(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.Say> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} message=\"{ev.Message}\" @ volume={ev.Volume}");
        }

        private void EventHandler_OnUtilityWriteToConsole(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Utility.WriteToConsole> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Message}");
        }

        private void EventHandler_OnInternalPluginStateChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginStateChanged> e)
        {
            var ev = e.Event;
            Debug.WriteLine($"$$ {ev} {ev.Id} {ev.PluginName} {ev.PluginStatus}");
        }

        private void EventHandler_OnDefault(EventHandler source, EventHandler.EventHandlerArgs<IEvent> e)
        {
            Debug.WriteLine($"$$ {e.Event}");
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
        }

        public void RegisterPlugin(IEngine engine)
        {
            EventBusSubscription = engine.RegisterListener();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            EventBusSubscription?.Dispose();
            EventBusSubscription = null;
        }

        public void Loop()
        {
            var e = EventBusSubscription?.NextEvent(250);

            if (Enabled)
            {
                EventHandler.HandleEvent(e);
            }
        }
    }
}
