# Lua

This provides two plugins. A LuaManagerPlugin and LuaPlugin.
 
LuaPlugin is a plugin that executes a single lua script. It takes care of
wiring events to it and provide whatever lua functionality is available.

LuaManagerPlugin coordinates the LuaPlugins in terms of launch new and taking
down old ones. It consumes data from FileMonitorPlugin and Engine primarily.

## Events

<details><summary>LuaCommandDeduplicateEvents</summary><br />
Send a set of Events, allowing the plugin to collect them and deduplicate them
before sending then. This is used at startup, to avoid requesting the same data
in multiple scripts.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `LuaCommandDeduplicateEvents` (constant)                          |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Events          | string  | JSON serialized version of the events. Separated by \n            |

**JSON Example:**  
`{"EventType": "LuaCommandDeduplicateEvents", "Uptime":1742, "Events": "JSON-ENCODED EVENTS"}`
</details>
