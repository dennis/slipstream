# FileMonitor

Monitors Scripts directory and sends out events in  case changes are detected. 

LuaManagerPlugin is the consumer of these events. If the extension is ".lua", it will launch
a LuaPlugin for the file. Similary, if the file is removed or renamed, it will stop the 
plugin again. This is the heart of the hot-reloading of lua scripts, every time a file is
changed.

## Lua

There is no Lua interface for this component.

## Events

<details><summary>FileMonitorCommandScan</summary><br />
Request that the monitored directories are scanned and existing files are sent as if they
were just created. This is used at startup.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorCommandScan` (constant)                               |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |


**JSON Example:** 
`{"EventType": "FileMonitorCommandScan", "ExcludeFromTxrx": true, "Uptime":1742}`
</details>

<details><summary>FileMonitorFileChanged</summary><br />
File modified

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileChanged` (constant)                               |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| FilePath        | string  | Name of the file changed                                          |

**JSON Example:** 
`{"EventType": "FileMonitorFileChanged",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorFileCreated</summary><br />
New file created

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileCreated` (constant)                               |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| FilePath        | string  | Name of the file created                                          |

**JSON Example:** 
`{ "EventType": "FileMonitorFileCreated", "ExcludeFromTxrx": true, "Uptime":1742, "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP" }`
</details>

<details><summary>FileMonitorFileDeleted</summary><br />
File deleted

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileDeleted` (constant)                               |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| FilePath        | string  | Name of the file deleted                                          |

**JSON Example:** 
`{"EventType": "FileMonitorFileDeleted",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP"}`
</details>

<details><summary>FileMonitorFileRenamed</summary><br />
File renamed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileRenamed` (constant)                               |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| FilePath        | string  | New name of the file                                              |
| OldFilePath     | string  | Previous name of the file                                         |

**JSON Example:** 
`{"EventType": "FileMonitorFileRenamed",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\debug.lua",  "OldFilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorScanCompleted</summary><br />
A FileMonitorCommandScan was completed.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorScanCompleted` (constant)                             |
| ExcludeFromTxrx | boolean | true (constant)                                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**  
`{"EventType": "FileMonitorScanCompleted", "ExcludeFromTxrx": true, "Uptime":1742}`
</details>
