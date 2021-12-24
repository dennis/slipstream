local web = require("api/webserver"):instance({ id = "web", port = 8888})

web:serve_content("/", "text/plain", "hello world")

local count = 0

addEventHandler("WebServerEndpointRequested", function(event)
    -- Example event: {"EventType":"WebServerEndpointRequested","Envelope":{"Sender":"web","Recipients":["Scripts\\webserver_example.lua"],"Uptime":6179},"Server":"web","Endpoint":"/","Method":"GET","Body":"","QueryParams":""}

    if event.Server == "web" and event.Endpoint == "/" then
        count = count + 1

        web:serve_content("/", "text/plain", "hello again #" .. count)
    end
end)
