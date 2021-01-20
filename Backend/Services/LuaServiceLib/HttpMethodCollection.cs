using NLua;
using Serilog;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public class HttpMethodCollection
    {
        private readonly ILogger Logger;

        public static HttpMethodCollection Register(ILogger logger, Lua lua)
        {
            var m = new HttpMethodCollection(logger);
            m.Register(lua);
            return m;
        }

        public HttpMethodCollection(ILogger logger)
        {
            Logger = logger;
        }

        public void Register(Lua lua)
        {
            lua["http"] = this;
            lua.DoString(@"
function post_as_json(u, b); http:post_as_json(u, b); end
");
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