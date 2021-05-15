# FileMonitor

Monitors one or more directory and sends out events in  case changes are detected. 

## Lua

<details><summary>Construction</summary><br />

```lua
local filemonitor = require("api/filemonitor"):instance(config)
```

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default     | Description                    |
| :---------- | :-----------: | :---------: | :----------------------------- |
| id          | string        |             | Mandatory: Id of this instance |
| paths       | string array  |             | Directories to monitor         |
</details>

<details><summary>filemonitor:scan()</summary><br />

```lua
local filemonitor = require("api/filemonitor"):instance(config)
filemonitor.scan()
```

Will scan the existing directories and send out files found.
</details>

## Events

<details><summary>FileMonitorCommandScan</summary><br />
Request that the monitored directories are scanned and existing files are sent as if they
were just created. This is used at startup.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorCommandScan` (constant)                               |
| InstanceId      | string  | Which instance the message originates from                        |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |


**JSON Example:** 
`{"EventType": "FileMonitorCommandScan", "Uptime":1742, "InstanceId":"FS"}`
</details>

<details><summary>FileMonitorFileChanged</summary><br />
File modified

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileChanged` (constant)                               |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| FilePath        | string  | Name of the file changed                                          |

**JSON Example:** 
`{"EventType": "FileMonitorFileChanged", "Uptime":1742, "InstanceId":"FS",  "FilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorFileCreated</summary><br />
New file created

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileCreated` (constant)                               |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| FilePath        | string  | Name of the file created                                          |

**JSON Example:** 
`{ "EventType": "FileMonitorFileCreated", "Uptime":1742, "InstanceId":"FS", "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP" }`
</details>

<details><summary>FileMonitorFileDeleted</summary><br />
File deleted

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileDeleted` (constant)                               |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| FilePath        | string  | Name of the file deleted                                          |

**JSON Example:** 
`{"EventType": "FileMonitorFileDeleted", "Uptime":1742, "InstanceId":"FS", "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP"}`
</details>

<details><summary>FileMonitorFileRenamed</summary><br />
File renamed

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorFileRenamed` (constant)                               |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |
| FilePath        | string  | New name of the file                                              |
| OldFilePath     | string  | Previous name of the file                                         |

**JSON Example:** 
`{"EventType": "FileMonitorFileRenamed", "Uptime":1742, "InstanceId":"FS", "FilePath": "Scripts\\debug.lua",  "OldFilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorScanCompleted</summary><br />
A FileMonitorCommandScan was completed.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `FileMonitorScanCompleted` (constant)                             |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| InstanceId      | string  | Which instance the message originates from                        |

**JSON Example:**  
`{"EventType": "FileMonitorScanCompleted", "Uptime":1742, "InstanceId":"FS"}`
</details>
