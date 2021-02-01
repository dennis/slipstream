# Discord

This is a very basic discord integration. It currently only supports receiving
messages and sending channel messages (possible using tts).

## Lua
 
You will need to get a token from Discord.

```
register_plugin({ plugin_name = "DiscordPlugin", token = "ODA....IQ8"})
```

<details><summary>discord:send_channel_message(params)</summary><br />

Send a message to a channel on discord. `params` is a table, that 
needs to contain:


| Parameter  | Type    | Description                                                    |
|:-----------|:-------:|:---------------------------------------------------------------|
| channel_id | int     | Which channel id to send message to                            |
| message    | string  | What to send                                                   |
| tts        | boolean | optional: Use discord TTS to speak the message (default false) |


```lua
discord:send_channel_message({ channel_id = 804666230932766730, message = "Hi From Lua", tts = false})
```
</details>

## Events

<details><summary>DiscordCommandSendMessage</summary><br />
Send a message to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordCommandSendMessage` (constant)                            |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| ChannelId       | long    | Which channel id to send message to                               |
| Message         | string  | What to send                                                      |
| TextToSpeech    | boolean | Use discord TTS to speak the message                              |

**JSON Example:**
`{"EventType":"DiscordCommandSendMessage","ExcludeFromTxrx":false,"Uptime":1398,"ChannelId":804666230932766730,"Message":"Hi From Lua","TextToSpeech":false}`
</details>

<details><summary>DiscordConnected</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordConnected` (constant)                                     |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"DiscordConnected","ExcludeFromTxrx":false,"Uptime":1391}`
</details>

<details><summary>DiscordDisconnected</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordDisconnected` (constant)                                  |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"DiscordDisconnected","ExcludeFromTxrx":false,"Uptime":1391}`
</details>

<details><summary>DiscordMessageReceived</summary><br />

We are connected to discord

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `DiscordMessageReceived` (constant)                               |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| From            | String  | User sending the message                                          |
| FromId          | long    | Discord User Id for "From"                                        |
| Message         | string  | Message sent                                                      |
| Channel         | string  | Channel message was sent in                                       |
| ChannelId       | long    | Discord Channel Id for "Channel"                                  |

**JSON Example:**
`{"EventType":"DiscordMessageReceived","ExcludeFromTxrx":false,"Uptime":265699,"From":"dennis#2358","FromId":395311112851292161,"Message":"Hello bot","Channel":"live-commentry","ChannelId":804666230932766730}`
</details>
