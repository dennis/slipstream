# Lua API

## Audio

These are provided by AudioPlugin.

<details><summary>audio:say(message, volume)</summary><br />
Use Windows text-to-speech to say message. 

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| message   | string      | What to say                  |
| volume    | number 0..1 | 0 is muted, 1 is full volume |

```lua
audio:say("Hello", 0.8)
```

This function publishes `AudioCommandSay` event, that is handled by AudioPlugin.

This function is aliased as ``say`` (deprecated)
</details>

<details><summary>audio:play(filename, volume)</summary><br />
Plays a wav/mp3 file relative to the audio directory (Open it via menu Help -> Open Audio Directory). 

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| filename  | string      | file to play                 |
| volume    | number 0..1 | 0 is muted, 1 is full volume |

```lua
audio:play("ding-sound-effect.mp3", 1.0)
```

This function publishes `AudioCommandPlay` event, that is handled by AudioPlugin.

This function is aliased as ``play`` (deprecated)
</details>

## Core

<details><summary>core:debounce(name, func, duration_seconds)</summary><br />
Debouncing a function ensures that it doesn’t get called too frequently. 

| Parameter        | Type     | Description                            |
|:-----------------|:--------:|:---------------------------------------|
| name             | string   | instance name (shared between scripts) |
| func             | function | function to invoke after duration      |
| duration_seconds | number   | duration before calling func           |

Everytime debounce is called, the timer is reset back to the duration. If you 
call debounce with another name a new timer is configured. Names are global, 
meaning that different scripts can use the same timer.

```lua
local hello = function()
    ui:print("Hello world")
end

core:debounce("sample1", hello, 10)

-- 9 seconds later. Timer is reset to 10 seconds again.
core:debounce("sample1", hello, 10)

-- 10 seconds later will output "Hello world"
```

This function is aliased as ``debounce`` (deprecated)
</details>

<details><summary>core:wait(name, func, duration_seconds)</summary><br />
Wait will wait for a duration before calling the function. If you redefine the wait, it 
will not reset the timer. This is the difference between this and debounce.

| Parameter        | Type     | Description                            |
|:-----------------|:--------:|:---------------------------------------|
| name             | string   | instance name (shared between scripts) |
| func             | function | function to invoke after duration      |
| duration_seconds | number   | duration before calling func           |

```lua
local hello = function()
    ui:print("Hello world")
end

core:wait("sample1", hello, 10)
core:wait("sample1", hello, 2)

-- 10 seconds later will output "Hello world"
```

This function is aliased as ``wait`` (deprecated)
</details>

<details><summary>core:event_to_json(event)</summary><br />
Encodes an event as a json-string.

| Parameter | Type  | Description                       |
|:----------|:-----:|:----------------------------------|
| event     | Event | Event as received from Slipstream |

```lua
function handle(event)
    core::print(core::event_to_json(event))
end
```

This function is aliased as ``event_to_json`` (deprecated)
</details>

## HTTP

<details><summary>http:post_as_json(event)</summary><br />
Performs a HTTP Post with payload and appropiate content-type.

| Parameter | Type   | Description  |
|:----------|:------:|:-------------|
| url       | string | URL endpoint |
| body      | string | JSON content |

Result will be written to the log.

This function is aliased as ``post_as_json`` (deprecated)
</details>

## Internals

<details><summary>internal:register_plugin(id, name)</summary><br />
Load a plugin. Used primarily in init.lua

| Parameter | Type   | Description    |
|:----------|:------:|:---------------|
| id        | string | plugin id      |
| name      | string | name of plugin |

```lua
internal:register_plugin("TwitchPlugin", "TwitchPlugin")
```

This function publishes `InternalCommandPluginRegister` event, that is handled by Engine.

This function is aliased as ``register_plugin`` (deprecated)
</details>

<details><summary>internal:unregister_plugin(id)</summary><br />
Removes a plugin

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| id        | string | plugin id   |

```lua
internal:unregister_plugin("TwitchPlugin")
```

This function publishes `InternalCommandPluginUnregister` event, that is handled by Engine.


This function is aliased as ``unregister_plugin`` (deprecated)
</details>

## IRacing

Provided by IRacingPlugin. Events are published as they happen. You can request specific data upon start, 
to be sure your script will see them. 

When IRacingPlugin can fetch data from IRacing it will issue a `IRacingConnected`. Similar when 
connection is lost a `IRacingDisconnected` is sent.

When connected to IRacing you might receive one or more of the following events:
 - `IRacingCarCompletedLap`
 - `IRacingCarInfo`
 - `IRacingCurrentSession`
 - `IRacingDriverIncident`
 - `IRacingPitEnter`
 - `IRacingPitExit`
 - `IRacingPitstopReport`
 - `IRacingRaceFlags`
 - `IRacingSessionState`
 - `IRacingTrackInfo`
 - `IRacingWeatherInfo`

<details><summary>iracing:send_car_info()</summary><br />
Request IRacingPlugin to send cars in session.

No arguments

```lua
iracing:send_car_info()
```

This function publishes `IRacingCommandSendCarInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_car_info`` (deprecated)
</details>

<details><summary>iracing:send_track_info()</summary><br />
Request IRacingPlugin to send track information.

No arguments

```lua
iracing:send_track_info()
```

This function publishes `IRacingCommandSendTrackInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_track_info`` (deprecated)
</details>

<details><summary>iracing:send_weather_info()</summary><br />
Request IRacingPlugin to send weather information.

No arguments

```lua
iracing:send_weather_info()
```

This function publishes `IRacingCommandSendWeatherInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_weather_info`` (deprecated)
</details>

<details><summary>iracing:send_current_session()</summary><br />
Request IRacingPlugin to send current session.

No arguments

```lua
iracing:send_current_session()
```

This function publishes `IRacingCommandSendCurrentSession` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_current_session`` (deprecated)
</details>

<details><summary>iracing:send_session_state()</summary><br />
Request IRacingPlugin to send session state

No arguments

```lua
iracing:send_session_state()
```

This function publishes `IRacingCommandSendSessionState` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_session_state`` (deprecated)
</details>

<details><summary>iracing:send_race_flags()</summary><br />
Request IRacingPlugin to send race flags

No arguments

```lua
iracing:send_race_flags()
```

This function publishes `IRacingCommandSendRaceFlags` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_race_flags`` (deprecated)
</details>

## State

State is a persistent key/value store. Both key and values most be strings. 
If an empty string is set, it will remove the entry.

State are global, so all scripts can access them.

<details><summary>state:get(key)</summary><br />
Retrieves a value for a given key.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| key       | string |             |

```lua
core::print(state:get("hello"))
```

This function is aliased as ``get_state`` (deprecated)
</details>

<details><summary>state:set(key, value)</summary><br />
Set a key/value pair, overwriting any existing key.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| key       | string |             |
| value     | string |             |

```lua
state:set("hello", "world")
core::print(state:get("hello"))
```

This function is aliased as ``set_state`` (deprecated)
</details>

<details><summary>state:set_temp(key, value, lifetime_in_seconds)</summary><br />
Set a key/value pair, overwriting any existing key. It will be auto-removed after lifetime_in_seconds.

| Parameter           | Type   | Description |
|:--------------------|:------:|:------------|
| key                 | string |             |
| value               | string |             |
| lifetime_in_seconds | number |             |

```lua
state:set_temp("hello", "world", 10)
core::print(state:get("hello"))
```

This function is aliased as ``set_temp_state`` (deprecated)
</details>

## Twitch

Very basic twitch integration. Provided by TwitchPlugin. It will send a `TwitchConnected` 
when connected and `TwitchDisconnected` when connection is lost. Messages prefixed with '!' 
is considered as a command, and will result `TwitchReceivedCommand` event.

<details><summary>twitch:send_channel_message(message)</summary><br />
Sends a public message to the twitch channel configured in settings.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| message   | string |             |

```lua
twitch:send_channel_message("Hello people")
```

This function publishes `TwitchCommandSendMessage` event, that is handled by TwitchPlugin.

This function is aliased as ``send_twitch_message`` (deprecated)
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

This function publishes `TwitchCommandSendWhisper` event, that is handled by TwitchPlugin.

This function is aliased as ``send_twitch_whisper`` (deprecated)
</details>

## UI

Very crude UI functionality. Adds/Removes buttons in the UI. Show text in console

<details><summary>ui:print(message)</summary><br />
Writes a string to the log shown in the UI.

| Parameter | Type   | Description     |
|:----------|:------:|:----------------|
| message   | string | string to write |

```lua
ui:print("Hello world")
```

This function publishes `UICommandWriteToConsole` event, that is handled by MainWindow.

This function is aliased as ``print`` (deprecated)
</details>

<details><summary>ui:create_button(label)</summary><br />
Creates a button if it doesn't exist. If it does exist, it is ignored.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| label     | string |             |

```lua
ui:create_button("Hello UI")
```

This function publishes `UICommandCreateButton` event, that is handled by MainWindow. `UIButtonTriggered` events will be sent, if
the button is pressed.

This function is aliased as ``create_button`` (deprecated)
</details>

<details><summary>ui:delete_button(label)</summary><br />
Deletes a button if it exists.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| label     | string |             |

```lua
ui:delete_button("Hello UI")
```

This function publishes `UICommandDeleteButton` event, that is handled by MainWindow.

This function is aliased as ``delete_button`` (deprecated)
</details>
