# Internal

## Lua: Core

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

This function is aliased as ``debounce``
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

This function is aliased as ``wait``
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
