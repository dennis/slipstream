# Playback

Enables saving and replaying of events. Slipstream stores up-to 10.000 
events that you may save into a plain text file (that you can, and 
should edit). After doing so, you can load these into slipstream again.

## Lua

<details><summary>Construction</summary><br />

```lua
local playback = require("api/playback"):instance(config)
```

This will construct an instance of `api/playback` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
</details>

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