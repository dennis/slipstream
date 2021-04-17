# Playback

Enables saving and replaying of events. Slipstream stores up-to 10.000 
events that you may save into a plain text file (that you can, and 
should edit). After doing so, you can load these into slipstream again.

## Lua

<details><summary>playback:load(filename)</summary><br />
Loads events from a file. Please note, that the events are not filtered, 
so if there are events that disables or enables plugins, this will 
be performed.

| Parameter | Type   | Description              |
|:----------|:------:|:-------------------------|
| filename  | string | file to load events from |


```lua
playback:load("test.mjson")
```

</details>
<details><summary>playback:save(filename)</summary><br />

Saves events to a file. This file can be edited as its a line-feed delimited 
json file. Each line is a event, so you can remove whatever you don't need. 

| Parameter | Type   | Description     |
|:----------|:------:|:----------------|
| filename  | string | store events as |

```lua
playback:save("test.mjson")
```
</details>

## Events

<details><summary>PlaybackCommandInjectEvents</summary><br />

Requests that a filename is read and sent as events.


| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| Filename         | string  | Filename to read events from|

**JSON Example:**
`{"EventType":"PlaybackCommandInjectEvents","Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T17.53.52.mjson","Uptime":4818}`
</details>

<details><summary>PlaybackCommandSaveEvents</summary><br />

Requests that events already seen, is stored in a file.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| Filename         | string  | Filename write events to|

**JSON Example:**
`{"EventType":"PlaybackSaveEvents","Uptime":73571,"Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T21.28.15.mjson"}`
</details>