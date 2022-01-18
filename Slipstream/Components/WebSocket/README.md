# WebSocket

A WebSocket client. Connect to an websocket to send and receive data to/from 
it.

## Lua


<details><summary>Construction</summary><br />

```lua
local ws = require("api/websocket"):instance(config)
```

This will construct an instance of `api/twitch` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| endpoint    | string        |            | Mandatory: ws:// or wss:// URL |

This will connect to the websocket, and you'll get events once it's done. Similar
there will be events for all data received.
</details>

<details><summary>send_data(data)</summary><br />
Sends data to an websocket endpoint. Data can a 
string or a LuaTable that will be serialized as json.

```lua
local web = require("api/websocket"):instance({ id = "ws", endpoint = "ws://localhost:9999"})
web:send_data({ text = "hello world" })
```
