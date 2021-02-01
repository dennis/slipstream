# Audio

This component provides lua function to play wave/mp3 audio files or text-to-speech.

## Lua

<details><summary>audio:say(plugin_id, message, volume)</summary><br />
Use Windows text-to-speech to speak message. 

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| plugin_id | string      | which plugin should act      |
| message   | string      | What to say                  |
| volume    | number 0..1 | 0 is muted, 1 is full volume |

```lua
audio:say("AudioPlugin", "Hello", 0.8)
```

This function publishes `AudioCommandSay` event, that is handled by AudioPlugin.

This function is aliased as ``say`` (deprecated)
</details>

<details><summary>audio:play(plugin_id, filename, volume)</summary><br />
Plays a wav/mp3 file relative to the audio directory (Open it via menu Help -> Open Audio Directory). 

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| plugin_id | string      | which plugin should act      |
| filename  | string      | file to play                 |
| volume    | number 0..1 | 0 is muted, 1 is full volume |

```lua
audio:play("AudioPlugin", "ding-sound-effect.mp3", 1.0)
```

This function publishes `AudioCommandPlay` event, that is handled by AudioPlugin.

This function is aliased as ``play`` (deprecated)
</details>

<details><summary>audio:send_devices(plugin_id)</summary><br />

Request that `AudioOutputDevice` is sent for each audio device found.

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| plugin_id | string      | which plugin should act      |

```lua
audio:send_devices("AudioPlugin")
```

This function publishes `AudioCommandSendDevices` event, that is handled by AudioPlugin.

</details>

<details><summary>audio:set_output(plugin_id, device_idx)</summary><br />
Change output for a plugin to another device. All plugins starts using 
default audio output, but can be changed via this. 

| Parameter  | Type   | Description                       |
|:-----------|:------:|:----------------------------------|
| plugin_id  | string | which plugin should act           |
| device_idx | number | what device to use. -1 is default |

```lua
audio:set_output("AudioPlugin", 2)
```

This function publishes `AudioCommandSetOutputDevice` event, that is handled by AudioPlugin.
</details>

## Events

<details><summary>AudioCommandPlay</summary><br />
Requests a mp3/wave files to be played. Filename is relative to the audio directory.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `AudioCommandPlay` (constant)                                     |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| PluginId        | string  | Which plugin should act                                           |
| Filename        | string  | Filename to play, relative to the audio directory                 |
| Volume          | numeric | Value from 0 .. 1, being from muted (0) to full volume (1)        |


**JSON Example:** 
`{"EventType": "AudioCommandPlay", "ExcludeFromTxrx": true, "Uptime":299, "PluginId": "AudioDefault", "Filename": "Ding-sound-effect.mp3", "Volume": 1}`
</details>

<details><summary>AudioCommandSay</summary><br />
Request message to read out loud using Windows text-to-speech

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `AudioCommandSay` (constant)                                      |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| PluginId        | string  | Which plugin should act                                           |
| Message         | string  | Text to speak                                                     |
| Volume          | numeric | Value from 0 .. 1, being from muted (0) to full volume (1)        |

**JSON Example:** 
`{"EventType": "AudioCommandSay",  "ExcludeFromTxrx": true, "Uptime":299,  "PluginId": "AudioDefault",  "Message": "Slipstream ready",  "Volume": 0.800000012}`
</details>

<details><summary>AudioCommandSendDevices</summary><br />

Send known devices via `AudioOutputDevice`.

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `AudioCommandSendDevices` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                     |
| Uptime   | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| PluginId        | string  | Which plugin should act              |

**JSON Example:** 
`{"EventType":"AudioCommandSendDevices","ExcludeFromTxrx":false,"PluginId":"AudioPlugin:Default"}`
</details>

<details><summary>AudioCommandSetOutputDevice</summary><br />

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `AudioCommandSetOutputDevice` (constant)                          |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| PluginId        | string  | Which plugin should act                                           |
| DeviceIdx       | int     | DeviceIdx to use for this plugin                                  |

**JSON Example:** 
`{"EventType":"AudioCommandSetOutputDevice","ExcludeFromTxrx":false,"PluginId":"AudioPlugin:Other","DeviceIdx":2}`
</details>

<details><summary>AudioOutputDevice</summary><br />

A output device found. Note: Device with DeviceIdx -1 is the default device.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `AudioOutputDevice` (constant)                                    |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| PluginId        | string  | Plugin responding                                                 |
| Product         | string  | Product as returned by Windows                                    |
| DeviceIdx       | int     | Device index, use this for selecting the device                   |

**JSON Example:** 
`{"EventType":"AudioOutputDevice","ExcludeFromTxrx":true,"PluginId":"AudioPlugin:Default","Product":"Microsoft Sound Mapper","DeviceIdx":-1}`
</details>
