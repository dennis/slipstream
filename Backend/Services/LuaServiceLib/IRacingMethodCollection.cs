﻿using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class IRacingMethodCollection
    {
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;

        public static IRacingMethodCollection Register(IEventBus eventBus, IIRacingEventFactory eventFactory, Lua lua)
        {
            var m = new IRacingMethodCollection(eventBus, eventFactory);
            m.Register(lua);
            return m;
        }

        public IRacingMethodCollection(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Register(Lua lua)
        {
            lua["iracing"] = this;
            lua.DoString(@"
function iracing_send_car_info(); iracing:send_car_info(); end
function iracing_send_track_info(); iracing:send_track_info(); end
function iracing_send_weather_info(); iracing:send_weather_info(); end
function iracing_send_current_session(); iracing:send_current_session(); end
function iracing_send_session_state(); iracing:send_session_state(); end
function iracing_send_race_flags(); iracing:send_race_flags(); end
");
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