#nullable enable

using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using Slipstream.Shared.Helpers.StrongParameters;
using Newtonsoft.Json;

namespace Slipstream.Components.WebWidget.Lua
{
    public class WebWidgetLuaReference : BaseLuaReference
    {
        private readonly IWebWidgetEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public WebWidgetLuaReference(string instanceId, string luaScriptInstanceId, IWebWidgetEventFactory eventFactory, IEventBus eventBus) : base(instanceId, luaScriptInstanceId)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send(LuaTable data)
        {
            var json = JsonConvert.SerializeObject(Parameters.From(data));
            EventBus.PublishEvent(EventFactory.CreateWebWidgetCommandEvent(Envelope, json));
        }
    }
}