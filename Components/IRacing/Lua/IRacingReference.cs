using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingReference : IIRacingReference
    {
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;

        public IRacingReference(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Dispose()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_car_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendCarInfo());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_track_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendTrackInfo());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_weather_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendWeatherInfo());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_session_state()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendSessionState());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_race_flags()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendRaceFlags());
        }
    }
}