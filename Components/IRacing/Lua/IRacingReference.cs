using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingReference : BaseLuaReference, IIRacingReference
    {
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;

        public IRacingReference(string instanceId, string luaScriptInstanceId, IEventBus eventBus, IIRacingEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void send_car_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendCarInfo(Envelope));
        }

        public void send_track_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendTrackInfo(Envelope));
        }

        public void send_weather_info()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendWeatherInfo(Envelope));
        }

        public void send_session_state()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendSessionState(Envelope));
        }

        public void send_race_flags()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandSendRaceFlags(Envelope));
        }

        public void pit_clear_all()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitClearAll(Envelope));
        }

        public void pit_clear_tyres_change()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitClearTyresChange(Envelope));
        }

        public void pit_request_fast_repair()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitRequestFastRepair(Envelope));
        }

        public void pit_add_fuel(int liters)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitAddFuel(Envelope, liters));
        }

        public void pit_change_left_front_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeLeftFrontTyre(Envelope, kpa));
        }

        public void pit_change_left_rear_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeLeftRearTyre(Envelope, kpa));
        }

        public void pit_change_right_front_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeRightFrontTyre(Envelope, kpa));
        }

        public void pit_change_right_rear_tyre(int kpa)
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitChangeRightRearTyre(Envelope, kpa));
        }

        public void pit_clean_windshield()
        {
            EventBus.PublishEvent(EventFactory.CreateIRacingCommandPitCleanWindshield(Envelope));
        }
    }
}