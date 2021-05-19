using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingReference : IIRacingReference
    {
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;
        private readonly IRacingLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public IRacingReference(string instanceId, IRacingLuaLibrary luaLibrary, IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }

        public void send_car_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendCarInfo());
        }

        public void send_track_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendTrackInfo());
        }

        public void send_weather_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendWeatherInfo());
        }

        public void send_session_state()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendSessionState());
        }

        public void send_race_flags()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendRaceFlags());
        }

        public void pit_clear_all()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitClearAll());
        }

        public void pit_clear_tyres_change()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitClearTyresChange());
        }

        public void pit_request_fast_repair()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitRequestFastRepair());
        }

        public void pit_add_fuel(int liters)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitAddFuel(liters));
        }

        public void pit_change_left_front_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeLeftFrontTyre(kpa));
        }

        public void pit_change_left_rear_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeLeftRearTyre(kpa));
        }

        public void pit_change_right_front_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeRightFrontTyre(kpa));
        }

        public void pit_change_right_rear_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeRightRearTyre(kpa));
        }

        public void pit_clean_windshield()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitCleanWindshield());
        }
    }
}