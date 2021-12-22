# WebServer

Enables you to create http endpoints hpsted by Slipstream. Currently two different 
endpoints can be defined: HTTP and WebSockets.

This can be used with OBS as a browswer source, allowing Slipstream to visually
display data on stream. Or you can make Slipstream act whenever an endpoint 
gets a request.

This component will keep the endpoint running until the instance is removed again.


## Lua

<details><summary>Construction</summary><br />

```lua
local web = require("api/web"):instance(config)
```

This will return a Web instance and create it if it does not exists.When doing so,
Slipstream will launch a webserver listning for requests. It will not support any
endpoints unless you add them with the `serve_` methods below.

`config` is the initial configuration of the instance if one needs to be created. 
It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                         |
| :---------- | :-----------: | :--------: | :---------------------------------- |
| id          | string        |            | Mandatory: Id of this instance      |
| port        | integer       |            | Mandatory: Which port to listen on  |


For example: 
```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
```

Will make Slipstream create a webserver, listening on http://127.0.0.1:8888. It doesn't
offer any endpoints, so we need to configure that. See the other functions exposed by `Web`.

<details><summary>web:serve_content(route, mimeType, content)</summary><br />
Will configure an endpoint, returning `content` as `mimeType`. 

```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
web:serve_content("/hello.txt", "text/plain", "Hello world")
```

Visiting http://127.0.0.1:8888/hello.txt - show you `Hello World`. It will also
generate a `WebEndpointRequested` event

<details><summary>web:serve_file(route, mimeType, filename)</summary><br />
Will configure an endpoint, returning the contents from `filename` as `mimeType`.  
This is an convenient method can could be implemented in pure lua using the 
`serve_content()` function instead.

```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
web:serve_file("/init.lua", "text/plain", "init.lua") -- relative to Slipstream main directory
```

Visiting http://127.0.0.1:8888/init.lua - show you the contents of your `init.lua`. 
It will also generate a `WebEndpointRequested` event

<details><summary>web:serve_directory(route, path)</summary><br />
Will configure an endpoint, serving all files within `path`. This can be useful
if you want to have assets such as images, javascript and css files externally, 
without needing to configure them one by one..

```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
web:serve_file("/scripts/", "Scripts") -- relative to Slipstream main directory
```

Visiting http://127.0.0.1:8888/scripts/test.lua - show you the contents of your `scripts/test.lua`. 
It will NOT generate a `WebEndpointRequested` event.

<details><summary>serve_websocket(route)</summary><br />
Adds a websocket endpoint. Use `send_data()` function to send data to it.

```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
web:serve_websocket("/ws")
```

<details><summary>send_data(route, data)</summary><br />
Sends data to an websocket endpoint. Data can a string or a LuaTable that will be
serialized as json.

```lua
local web = require("api/web"):instance({ id = "web", port = 8888})
web:send_data("/ws", { text = "hello world" })
```