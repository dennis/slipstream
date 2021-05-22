# Twitch

Very basic twitch integration.  It will send a
`TwitchConnected` when connected and `TwitchDisconnected` when connection is
lost. Messages prefixed with '!' is considered as a command, and will result
`TwitchReceivedCommand` event.

It handles throttling, so if too many messages are sent, it will slown them
down.  If the twitch user configured is an operator, you will get a bit less
restrictive throttling by twitch.

## Lua


<details><summary>Construction</summary><br />

```lua
local twitch = require("api/twitch"):instance(config)
```

This will construct an instance of `api/twitch` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| username    | string        |            | Twitch username                |
| token       | string        |            | Twitch token                   |
| channel     | string        |  username  | Twitch channel                 |

Token can be generated here: https://twitchapps.com/tmi/
</details>

<details><summary>twitch:send_channel_message(message)</summary><br />
Sends a public message to the twitch channel configured in settings.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| message   | string |             |

```lua
twitch:send_channel_message("Hello people")
```
</details>

<details><summary>twitch:send_whisper_message(to, message)</summary><br />
Sends a public message to a  twitch user.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| to        | string | Recipient   |
| message   | string |             |

```lua
twitch:send_whisper_message("tntion", "Hello people")
```

This function publishes `TwitchCommandSendWhisper` event, that is handled by
TwitchPlugin.

This function is aliased as ``send_twitch_whisper`` (deprecated)
</details>
