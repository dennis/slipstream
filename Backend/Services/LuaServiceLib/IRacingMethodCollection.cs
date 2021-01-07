using NLua;
using Slipstream.Shared;

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class IRacingMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;

            public static IRacingMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, Lua lua)
            {
                var m = new IRacingMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            private IRacingMethodCollection(IEventBus eventBus, IEventFactory eventFactory)
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
        }
    }
}
