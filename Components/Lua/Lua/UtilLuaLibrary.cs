#nullable enable

using NLua;
using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Slipstream.Components.Lua.Lua
{
    public class UtilLuaLibrary : ILuaLibrary
    {
        private readonly IEventSerdeService EventSerdeService;
        private readonly ILogger Logger;

        public string Name => "api/util";

        public UtilLuaLibrary(IEventSerdeService eventSerdeService, ILogger logger)
        {
            EventSerdeService = eventSerdeService;
            Logger = logger;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfg)
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public string event_to_json(IEvent @event)
        {
            return EventSerdeService.Serialize(@event);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void post_as_json(string uri, string body)
        {
            Task.Run(() => PostJson(new Uri(uri), body));
        }

        private async Task PostJson(Uri uri, string body)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Logger.Verbose("post_as_json response {Response}", responseContent);
            }
            else
            {
                Logger.Error("post_as_json ERROR: uri: {Uri}\nbody: {Body}\n {(int)StatusCode} - {ReasonPhrase}", uri, body, (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}