# Audio

This component provides a lua library to play wave/mp3 audio files or text-to-speech.

## Lua

<details><summary>Construction</summary><br />

```lua
local audio = require("api/audio"):instance(config)
```

This will construct an instance of `api/audio` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. 
It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| path        | string        | Audio      | Path to audio file             |
| output      | integer       | -1         | DeviceIdx to use as output     |
</details>

<details><summary>audio:say(message)</summary><br />

Use Windows text-to-speech to speak message. 

```lua
local audio = require("api/audio"):instance(config)
audio:say("Hello")
```

| Parameter | Type        | Description                  |
|:----------|:-----------:|:-----------------------------|
| message   | string      | What to say                  |

This function is implemented using `AudioCommandSay` event.
</details>

<details><summary>audio:play(filename)</summary><br />
Plays a wav/mp3 file relative to the audio directory (as set in the construction, defaults to  `Audio`)

| Parameter   | Type          | Description                    |
| :---------- | :-----------: | :----------------------------- |
| filename    | string        | file to play                   |

```lua
local audio = require("api/audio"):instance(config)
audio:play("ding-sound-effect.mp3")
```

This function is implemented using `AudioCommandPlay`.
</details>

<details><summary>audio:send_devices()</summary><br />

```lua
local audio = require("api/audio"):instance(config)
audio:send_devices()
```

Request that `AudioOutputDevice` events are sent for each audio device found.

This function is implemented using `AudioCommandSendDevices` event, that in turn will 
sent multiple `AudioOutputDevice` events.
</details>

<details><summary>audio:set_output(device_idx)</summary><br />

```lua
local audio = require("api/audio"):instance(config)
audio:set_output(2)
```

Change output for an instance to another device. All instances starts using 
default audio output, but can be changed via this. 

| Parameter  | Type   | Description                       |
|:-----------|:------:|:----------------------------------|
| device_idx | number | what device to use. -1 is default |


This function is implemented using `AudioCommandSetOutputDevice` event.

See `audio:send_devices()` to get events describing the devices available.
</details>

<details><summary>audio:set_volume(device_idx)</summary><br />

```lua
local audio = require("api/audio"):instance(config)
audio:set_volume(0.5)
```

Change output for an instance to another device. All instances starts using 
default audio output, but can be changed via this. 

| Parameter  | Type   | Description                       |
|:-----------|:------:|:----------------------------------|
| volume     | number 0..1 | 0 is muted, 1 is full volume |

The volume is only set for the current instance. If you have another
script using the same instance that volume will not be changed for that
script.
</details>
