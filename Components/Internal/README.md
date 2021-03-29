# Internal

## Lua: Core

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

## Lua: HTTP

<details><summary>http:post_as_json(event)</summary><br />
Performs a HTTP Post with payload and appropiate content-type.

| Parameter | Type   | Description  |
|:----------|:------:|:-------------|
| url       | string | URL endpoint |
| body      | string | JSON content |

Result will be written to the log.

This function is aliased as ``post_as_json`` (deprecated)
</details>

## Lua: Internal

<details><summary>internal:register_plugin(args)</summary><br />
Load a plugin. Used primarily in init.lua

Args is a LUA table, with the following keys

| Parameter   | Type   | Mandatory | Description                         |
|:------------|:------:|:----------|:------------------------------------|
| plugin_id   | string | No        | plugin id (defaults to plugin_name) |
| plugin_name | string | Yes       | name of plugin                      |

```lua
internal:register_plugin({ plugin_name = "TwitchPlugin" })
internal:register_plugin({ plugin_id = "TwitchPlugin", plugin_name = "TwitchPlugin" })
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

## Lua: State

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

## Events

Do not depend on these. They are internal-

These are generated and consumed by Engine.

<details><summary>InternalCommandPluginRegister</summary><br />
Request a plugin to be consumed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalCommandPluginRegister` (constant)                        |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Id              | string  | Id of the Plugin                                                  |
| PluginName      | string  | Name of the Plugin                                                |
| Configuration   | string  | JSON-encoded configuration                                        |

**JSON Example:**  
`{ "EventType": "InternalCommandPluginRegister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin"}`
</details>

<details><summary>InternalCommandPluginStates</summary><br />
Request Engine to send state of all plugins

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalCommandPluginStates` (constant)                          |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType": "InternalCommandPluginStates", "ExcludeFromTxrx": true}`
</details>

<details><summary>InternalCommandPluginUnregister</summary><br />
Request a plugin to be removed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalCommandPluginUnregister` (constant)                      |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Id              | string  | Id of the Plugin                                                  |

**JSON Example:**
`{"EventType": "InternalCommandPluginUnregister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin" }`
</details>

<details><summary>InternalPluginState</summary><br />
Show the state of a plugin. These are published when plugins are 
registered or unregistered or upon request.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalPluginState` (constant)                                  |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Id              | string  | Id of plugin                                                      |
| PluginName      | string  | Name of plugin                                                    |
| DisplayName     | string  | Friendly name of the plugin (defaults to PluginName)              |
| PluginStatus    | string  | `Registered` or `Unregistered`                                    |

**JSON Example:**
`{"EventType": "InternalPluginState", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin", "DisplayName": "AudioPlugin", "PluginStatus": "Registered"}`
</details>

<details><summary>InternalCommandShutdown</summary><br />
Request that Slipstream should shut down.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalCommandShutdown` (constant)                              |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"InternalCommandShutdown","ExcludeFromTxrx":true,"Uptime":18396}`
</details>

<details><summary>InternalShutdown</summary><br />
Slipstream is shutting down.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `InternalShutdown` (constant)                                     |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"InternalShutdown","ExcludeFromTxrx":true,"Uptime":18396}`
</details>
