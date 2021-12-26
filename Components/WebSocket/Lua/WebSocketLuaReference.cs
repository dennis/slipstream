using Newtonsoft.Json;

using NLua;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.WebSocket.Lua
{
    public class WebSocketLuaReference : BaseLuaReference, IWebSocketLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IEventFactory EventFactory;

        public WebSocketLuaReference(
            string instanceId,
            string luaScriptInstanceId,
            IEventBus eventBus,
            IEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_data(string data)
        {
            EventBus.PublishEvent(EventFactory.CreateWebSocketCommandData(Envelope, data));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_data(LuaTable data)
        {
            var json = JsonConvert.SerializeObject(Parameters.From(data));
            send_data(json);
        }
    }
}