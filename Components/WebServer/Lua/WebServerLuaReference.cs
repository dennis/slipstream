#nullable enable

using Newtonsoft.Json;

using NLua;

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.WebServer.Lua
{
    public class WebServerLuaReference : BaseLuaReference
    {
        private readonly IWebServerEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public WebServerLuaReference(string instanceId, string luaScriptInstanceId, IWebServerEventFactory eventFactory, IEventBus eventBus) : base(instanceId, luaScriptInstanceId)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void serve_content(string route, string mimeType, string content)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandRouteStaticContent(Envelope, route, mimeType, content));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void serve_directory(string route, string path)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandRoutePath(Envelope, route, path));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void serve_websocket(string route)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandRouteWebSocket(Envelope, route));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_data(string route, string clientId, string data)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandData(Envelope, route, clientId, data));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_data(string route, string clientId, LuaTable data)
        {
            var json = JsonConvert.SerializeObject(Parameters.From(data));
            send_data(route, clientId, json);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void broadcast_data(string route, string data)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandData(Envelope, route, data));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void broadcast_data(string route, LuaTable data)
        {
            var json = JsonConvert.SerializeObject(Parameters.From(data));
            broadcast_data(route, json);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void serve_file(string route, string mimeType, string filename)
        {
            EventBus.PublishEvent(EventFactory.CreateWebServerCommandRouteFileContent(Envelope, route, mimeType, filename));
        }
    }
}