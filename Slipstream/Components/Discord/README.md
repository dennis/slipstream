# Discord

This is a very basic discord integration. It currently only supports receiving
messages and sending channel messages (possible using tts).

## Lua


<details><summary>Construction</summary><br />

```lua
local audio = require("api/audio"):instance(config)
```

This will construct an instance of `api/audio` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| token       | string        |            | Discord token                  |

You can register you bot and get a token at https://discord.com/developers/applications
</details>

<details><summary>discord:channel_id(channel_id)</summary><br />

```lua
local audio = require("api/audio"):instance(config)
local channel = audio.channel_id(123453434)
```

This will return an object for the channel. With this, you can send messages.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| channel_id  | integer       |            | Discord Channel Id             |

To get the channel id of a channel:

 - Open your Discord App
 - Go to User Settings
 - Choose Advanced under App Settings
 - Enable "Developer Mode"
 - Exit settings
 - Right click on the desired channel, choose "Copy ID"
</details>

<details><summary>channel:send_message(message)</summary><br />

```lua
local channel = require("api/audio"):instance(config).channel_id(123453434)
channel.send_message("Hello world")
```

Send a message to a channel on discord. 

| Parameter  | Type    | Description                                                    |
|:-----------|:-------:|:---------------------------------------------------------------|
| message    | string  | What to send                                                   |
</details>

<details><summary>channel:send_message_tts(message)</summary><br />

```lua
local channel = require("api/audio"):instance(config).channel_id(123453434)
channel.send_message_tts("Hello world")
```

Send a message to a channel on discord, with tts enabled

| Parameter  | Type    | Description                                                    |
|:-----------|:-------:|:---------------------------------------------------------------|
| message    | string  | What to send                                                   |
</details>
