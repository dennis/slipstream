using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.JustGiving.Lua
{
    public class JustGivingLuaReference : BaseLuaReference, IJustGivingLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IJustGivingEventFactory EventFactory;

        public JustGivingLuaReference(string instanceId, string luaScriptInstanceId, IEventBus eventBus, IJustGivingEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void send_all_donations()
        {
            EventBus.PublishEvent(EventFactory.CreateJustGivingCommandSendDonations(Envelope));
        }
    }
}