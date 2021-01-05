using NLua;
using Slipstream.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class HttpMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;

            public static HttpMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, Lua lua)
            {
                var m = new HttpMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            private HttpMethodCollection(IEventBus eventBus, IEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
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
                using(var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(request);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"post_as_json response: {responseContent}"));
                    }
                }
            }
        }
    }
}
