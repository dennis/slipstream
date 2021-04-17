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

## Events

<details><summary>DiscordCommandSendMessage</summary><br />
Send a message to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordCommandSendMessage` (constant)                            |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| ChannelId       | long    | Which channel id to send message to                               |
| Message         | string  | What to send                                                      |
| TextToSpeech    | boolean | Use discord TTS to speak the message                              |

**JSON Example:**
`{"EventType":"DiscordCommandSendMessage","Uptime":1398,"InstanceId":"discord","ChannelId":804666230932766730,"Message":"Hi From Lua","TextToSpeech":false}`
</details>

<details><summary>DiscordConnected</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordConnected` (constant)                                     |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |

**JSON Example:**
`{"EventType":"DiscordConnected","Uptime":1391,"InstanceId":"discord"}`
</details>

<details><summary>DiscordDisconnected</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordDisconnected` (constant)                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |

**JSON Example:**
`{"EventType":"DiscordDisconnected","Uptime":1391,"InstanceId":"discord"}`
</details>

<details><summary>DiscordMessageReceived</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordMessageReceived` (constant)                               |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| From            | String  | User sending the message                                          |
| FromId          | long    | Discord User Id for "From"                                        |
| Message         | string  | Message sent                                                      |
| Channel         | string  | Channel message was sent in                                       |
| ChannelId       | long    | Discord Channel Id for "Channel"                                  |

**JSON Example:**
`{"EventType":"DiscordMessageReceived","Uptime":265699,"InstanceId":"discord","From":"dennis#2358","FromId":395311112851292161,"Message":"Hello bot","Channel":"live-commentry","ChannelId":804666230932766730}`
</details>
