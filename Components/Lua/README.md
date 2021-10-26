# Internal

## Lua (callbacks)

<details><summary>handle(event)</summary><br />
Function you define in your script which will be invoked by Slipstream for
all events received for you.  `event` always contains an `event.EventType`
so you can tell them apart. A script can only have one `handle(event)` 
function.

Instead of using `handle(event)` directly, you can instead use the 
helper `addEventHandler(eventType, callback)`. It will wire everything up
and invoke your callback function, when an event of that type is received.
You can have multiple callbacks for the same event type if needed. As 
`addEventHandler` defines its own `handle(event)` function, you cannot
use it yourself.

<details><summary>atexit()</summary><br />
This function, if it is defined, will be invoked when the script shuts down.
You can't stop it from shutting down, but you can do some cleanup if needed.

## Lua (global functions)

<details><summary>debounce(name, func, duration_seconds)</summary><br />
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

debounce("sample1", hello, 10)

-- 9 seconds later. Timer is reset to 10 seconds again.
debounce("sample1", hello, 10)

-- 10 seconds later will output "Hello world"
```
</details>

<details><summary>wait(name, func, duration_seconds)</summary><br />
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

wait("sample1", hello, 10)
wait("sample1", hello, 2)

-- 10 seconds later will output "Hello world"
```
</details>


<details><summary>parse_json(json)</summary><br />
Will parse `json` and return a LuaTable with the values
parsed.

| Parameter        | Type     | Description                       |
|:-----------------|:--------:|:----------------------------------|
| json             | string   | json string to be parsed          |

```lua
local json = parse_json("{\"foo\":\"bar\"}")
json.foo -- "contains now bar"
```
</details>

<details><summary>generate_json(table)</summary><br />
The inverse of `parse_json()`, taking a lua table and return it as 
a json encoded string.

| Parameter        | Type      | Description    |
|:-----------------|:------+--:|:---------------|
| table            | luatable  | data           |

```lua
local json = generate_json({ foo = "bar" })
json -- "{\"foo\":\"bar\"}"
```
</details>

<details><summary>event_to_json(event)</summary><br />
Encodes an event as a json-string.

| Parameter | Type  | Description                       |
|:----------|:-----:|:----------------------------------|
| event     | Event | Event as received from Slipstream |

```lua
function handle(event)
    local json = event_to_json(event)
end
```
</details>

## Lua: Util

<details><summary>Construction</summary><br />

```lua
local util = require("api/util"):instance(config)
```

This will construct an instance of `api/util` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |

</details>

<details><summary>util:post_as_json(event)</summary><br />
Performs a HTTP Post with payload and appropiate content-type.

| Parameter | Type   | Description  |
|:----------|:------:|:-------------|
| url       | string | URL endpoint |
| body      | string | JSON content |

Result will be written to the log.
</details>

## Lua: Internal


<details><summary>Construction</summary><br />

```lua
local internal = require("api/internal"):instance(config)
```

This will construct an instance of `api/internal` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |

</details>

<details><summary>internal:shutdown()</summary><br />
Shutsdown Slipstream

```lua
internal:shutdown()
```

This function publishes `InternalCommandShutdown` event, that is handled by Engine, that will
publish `InternalShutdown` if it allows the shutdown.
</details>

<details><summary>internal:send_custom_event(name, payload)</summary><br />
Sends a custom event

| Parameter        | Type      | Description                |
|:-----------------|:---------:|:---------------------------|
| name             | string    | name to identify the event |
| payload          | string OR lua table    | payload to send in the event. If luatable is provided it will be converted to json via `generate_json()` |

```lua
internal:send_custom_event("command", { type = "sof" })
```
</details>

## Lua: State

State is a persistent key/value store. Both key and values most be strings. 
If an empty string is set, it will remove the entry.

State are global, so all scripts can access them.

<details><summary>Construction</summary><br />
```lua
local state = require("api/internal"):instance(config)
```

This will construct an instance of `api/internal` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
</details>


<details><summary>state:get(key)</summary><br />
Retrieves a value for a given key.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| key       | string |             |

```lua
core::print(state:get("hello"))
```
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
</details>
