# Events

Internally, Slipstream is event based. There are two different types of events. 
One describes something that happen. The other describes a request for changing 
something. These are non-command (describes what happened) and c
ommand-events (request changes).

Each event contains at least three properties. 
 - **EventType**: Name of the event (Such as AudioCommandPlay)
 - **ExcludeFromTxrx**: Is event transmitted via TransmitterPlugin
 - **Uptime**: Time of when the message was sent via Eventbus (in milliseconds). 
   Time is since start of EventBus

Both properties are constants.

In Lua you can receive these events, by implementuing a function called `handle` 
taking one argument, which is the event:

```lua
function handle(event)
  if event.EventType == "TwitchReceivedMessage" then
    if(event.Message == "!car") then
      twitch:send_channel_message("Look at the screen")
    end
  end
end
```

The above example will act on `TwitchReceivedMessage`, where if event contains 
the message `!car` it will respond with some static text.

## Audio

Provided by AudioPlugin. Handles text-to-speech and playing audio files.

<details><summary>AudioCommandPlay</summary><br />
Requests a mp3/wave files to be played. Filename is relative to the audio directory.

| Name            | Type    | Description                                                |
|:----------------|:-------:|:-----------------------------------------------------------|
| EventType       | string  | `AudioCommandPlay` (constant)                              |
| ExcludeFromTxrx | boolean | false (constant)                                           |
| PluginId        | string  | Which plugin should act                                    |
| Filename        | string  | Filename to play, relative to the audio directory          |
| Volume          | numeric | Value from 0 .. 1, being from muted (0) to full volume (1) |


**JSON Example:** 
`{"EventType": "AudioCommandPlay", "ExcludeFromTxrx": true, "Uptime":299, "PluginId": "AudioDefault", "Filename": "Ding-sound-effect.mp3", "Volume": 1}`
</details>

<details><summary>AudioCommandSay</summary><br />
Request message to read out loud using Windows text-to-speech

| Name            | Type    | Description                                                |
|:----------------|:-------:|:-----------------------------------------------------------|
| EventType       | string  | `AudioCommandSay` (constant)                               |
| ExcludeFromTxrx | boolean | false (constant)                                           |
| PluginId        | string  | Which plugin should act                                    |
| Message         | string  | Text to speak                                              |
| Volume          | numeric | Value from 0 .. 1, being from muted (0) to full volume (1) |

**JSON Example:** 
`{"EventType": "AudioCommandSay",  "ExcludeFromTxrx": true, "Uptime":299,  "PluginId": "AudioDefault",  "Message": "Slipstream ready",  "Volume": 0.800000012}`
</details>

<details><summary>AudioCommandSendDevices</summary><br />

Send known devices via `AudioOutputDevice`.

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `AudioCommandSendDevices` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                     |
| PluginId        | string  | Which plugin should act              |

**JSON Example:** 
`{"EventType":"AudioCommandSendDevices","ExcludeFromTxrx":false,"PluginId":"AudioPlugin:Default"}`
</details>

<details><summary>AudioCommandSetOutputDevice</summary><br />

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `AudioCommandSetOutputDevice` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                         |
| PluginId        | string  | Which plugin should act                  |
| DeviceIdx       | int     | DeviceIdx to use for this plugin         |

**JSON Example:** 
`{"EventType":"AudioCommandSetOutputDevice","ExcludeFromTxrx":false,"PluginId":"AudioPlugin:Other","DeviceIdx":2}`
</details>

<details><summary>AudioOutputDevice</summary><br />

A output device found. Note: Device with DeviceIdx -1 is the default device.

| Name            | Type    | Description                                     |
|:----------------|:-------:|:------------------------------------------------|
| EventType       | string  | `AudioOutputDevice` (constant)                  |
| ExcludeFromTxrx | boolean | true (constant)                                 |
| PluginId        | string  | Plugin responding                               |
| Product         | string  | Product as returned by Windows                  |
| DeviceIdx       | int     | Device index, use this for selecting the device |

**JSON Example:** 
`{"EventType":"AudioOutputDevice","ExcludeFromTxrx":true,"PluginId":"AudioPlugin:Default","Product":"Microsoft Sound Mapper","DeviceIdx":-1}`
</details>

## FileMonitor

Provided by FileMonitorPlugin. Monitors Scripts directory and sends out events in 
case changes are detected. 

LuaManagerPlugin is the consumer of these events. If the extension is ".lua", it will launch
a LuaPlugin for the file. Similary, if the file is removed or renamed, it will stop the 
plugin again. This is the heart of the hot-reloading of lua scripts, every time a file is
changed.

<details><summary>FileMonitorCommandScan</summary><br />
Request that the monitored directories are scanned and existing files are sent as if they
were just created. This is used at startup.

| Name            | Type    | Description                         |
|:----------------|:-------:|:------------------------------------|
| EventType       | string  | `FileMonitorCommandScan` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                     |

**JSON Example:** 
`{"EventType": "FileMonitorCommandScan", "ExcludeFromTxrx": true, "Uptime":1742}`
</details>

<details><summary>FileMonitorFileChanged</summary><br />
File modified

| Name            | Type    | Description                         |
|:----------------|:-------:|:------------------------------------|
| EventType       | string  | `FileMonitorFileChanged` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                     |
| FilePath        | string  | Name of the file changed            |

**JSON Example:** 
`{"EventType": "FileMonitorFileChanged",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorFileCreated</summary><br />
New file created

| Name            | Type    | Description                         |
|:----------------|:-------:|:------------------------------------|
| EventType       | string  | `FileMonitorFileCreated` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                     |
| FilePath        | string  | Name of the file created            |

**JSON Example:** 
`{ "EventType": "FileMonitorFileCreated", "ExcludeFromTxrx": true, "Uptime":1742, "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP" }`
</details>

<details><summary>FileMonitorFileDeleted</summary><br />
File deleted

| Name            | Type    | Description                         |
|:----------------|:-------:|:------------------------------------|
| EventType       | string  | `FileMonitorFileDeleted` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                     |
| FilePath        | string  | Name of the file deleted            |

**JSON Example:** 
`{"EventType": "FileMonitorFileDeleted",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\debug.lua~RF3bee738a.TMP"}`
</details>

<details><summary>FileMonitorFileRenamed</summary><br />
File renamed

| Name            | Type    | Description                         |
|:----------------|:-------:|:------------------------------------|
| EventType       | string  | `FileMonitorFileRenamed` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                     |
| FilePath        | string  | New name of the file                |
| OldFilePath     | string  | Previous name of the file           |

**JSON Example:** 
`{"EventType": "FileMonitorFileRenamed",  "ExcludeFromTxrx": true, "Uptime":1742,  "FilePath": "Scripts\\debug.lua",  "OldFilePath": "Scripts\\xiiv45pp.cru~"}`
</details>

<details><summary>FileMonitorScanCompleted</summary><br />
A FileMonitorCommandScan was completed.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `FileMonitorScanCompleted` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                       |

**JSON Example:**  
`{"EventType": "FileMonitorScanCompleted", "ExcludeFromTxrx": true, "Uptime":1742}`
</details>

## LuaManager

This coordinates the LuaPlugins in terms of launch new and taking down old ones. It consumes
data from FileMonitorPlugin and Engine primarily.

<details><summary>LuaCommandDeduplicateEvents</summary><br />
Send a set of Events, allowing the plugin to collect them and deduplicate them before
sending then. This is used at startup, to avoid requesting the same data in multiple 
scripts.

| Name            | Type    | Description                                            |
|:----------------|:-------:|:-------------------------------------------------------|
| EventType       | string  | `LuaCommandDeduplicateEvents` (constant)               |
| ExcludeFromTxrx | boolean | true (constant)                                        |
| Events          | string  | JSON serialized version of the events. Separated by \n |

**JSON Example:**  
`{"EventType": "LuaCommandDeduplicateEvents",  "ExcludeFromTxrx": true, "Uptime":1742, "Events": "JSON-ENCODED EVENTS"}`
</details>

## Internal

Do not depend on these. They are internal-

These are generated and consumed by Engine.


<details><summary>InternalCommandPluginRegister</summary><br />
Request a plugin to be consumed

| Name            | Type    | Description                                |
|:----------------|:-------:|:-------------------------------------------|
| EventType       | string  | `InternalCommandPluginRegister` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                            |
| Id              | string  | Id of the Plugin                           |
| PluginName      | string  | Name of the Plugin                         |
| Configuration   | string  | JSON-encoded configuration                 |

**JSON Example:**  
`{ "EventType": "InternalCommandPluginRegister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin"}`
</details>

<details><summary>InternalCommandPluginStates</summary><br />
Request Engine to send state of all plugins

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `InternalCommandPluginStates` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                          |

**JSON Example:**
`{"EventType": "InternalCommandPluginStates", "ExcludeFromTxrx": true}`
</details>

<details><summary>InternalCommandPluginUnregister</summary><br />
Request a plugin to be removed

| Name            | Type    | Description                                  |
|:----------------|:-------:|:---------------------------------------------|
| EventType       | string  | `InternalCommandPluginUnregister` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                              |
| Id              | string  | Id of the Plugin                             |

**JSON Example:**
`{"EventType": "InternalCommandPluginUnregister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin" }`
</details>

<details><summary>InternalPluginState</summary><br />
Show the state of a plugin. These are published when plugins are 
registered or unregistered or upon request.

| Name            | Type    | Description                                          |
|:----------------|:-------:|:-----------------------------------------------------|
| EventType       | string  | `InternalPluginState` (constant)                     |
| ExcludeFromTxrx | boolean | true (constant)                                      |
| Id              | string  | Id of plugin                                         |
| PluginName      | string  | Name of plugin                                       |
| DisplayName     | string  | Friendly name of the plugin (defaults to PluginName) |
| PluginStatus    | string  | `Registered` or `Unregistered`                       |

**JSON Example:**
`{"EventType": "InternalPluginState", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin", "DisplayName": "AudioPlugin", "PluginStatus": "Registered"}`
</details>

## IRacing

<details><summary>IRacingCarCompletedLap</summary><br />

Published every time a driver completes a full lap.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingCarCompletedLap` (constant)      |
| ExcludeFromTxrx | boolean | false (constant)                         |
| SessionTime     | float   | Time of event (seconds into the session) |
| CarIdx          | integer | Id of car                                |
| Time            | float   | Lap time                                 |
| LapsCompleted   | integer | How many laps were completed             |
| FuelDiff        | float   | Changes in fuel levels                   |
| LocalUser       | boolean | Is it our car?                           |

**JSON Example:**
`{"EventType":"IRacingCarCompletedLap","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":7306.3000976104477,"CarIdx":5,"Time":7306.3000976104477,"LapsCompleted":9,"FuelDiff":null,"LocalUser":false}`
</details>

<details><summary>IRacingCarInfo</summary><br />
Info about a new car or car with changed details (such as driver).

| Name                 | Type    | Description                                                     |
|:---------------------|:-------:|:----------------------------------------------------------------|
| EventType            | string  | `IRacingCarCompletedLap` (constant)                             |
| ExcludeFromTxrx      | boolean | false (constant)                                                |
| SessionTime          | float   | Time of event (seconds into the session)                        |
| CarNumber            | string  | Car's number                                                    |
| CurrentDriverUserID  | long    | IRacing Customer Id                                             |
| CurrentDriverName    | string  | Driver's full name                                              |
| TeamID               | long    | IRacing's team Id                                               |
| TeamName             | string  | IRacing's team name (might be same as Drivers name, if no team) |
| CarName              | string  | Full name of car                                                |
| CarNameShort         | string  | Short name of car                                               |
| CurrentDriverIRating | long    | Drivers IRating                                                 |
| CurrentDriverLicense | string  | Drivers License                                                 |
| LocalUser            | bool    | Is it our car?                                                  |
| Spectator            | bool    | Is car a spectator                                              |

**JSON Example:**
`{"EventType":"IRacingCarInfo","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"CarIdx":63,"CarNumber":"042","CurrentDriverUserID":411093,"CurrentDriverName":"Dennis M\u00F8llegaard Pedersen","TeamID":0,"TeamName":"Dennis M\u00F8llegaard Pedersen","CarName":"Mazda MX-5 Cup","CarNameShort":"MX-5 Cup","CurrentDriverIRating":1592,"CurrentDriverLicense":"A 4.50","LocalUser":true,"Spectator":true}`
</details>

<details><summary>IRacingCommandSendCarInfo</summary><br />

Request IRacingPlugin to send Car Info.

| Name            | Type    | Description                            |
|:----------------|:-------:|:---------------------------------------|
| EventType       | string  | `IRacingCommandSendCarInfo` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                       |

**JSON Example:**
`{"EventType":"IRacingCommandSendCarInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendRaceFlags</summary><br />
Request IRacingPlugin to send Race Flags.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingCommandSendRaceFlags` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                         |

**JSON Example:**
`{"EventType":"IRacingCommandSendRaceFlags","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendSessionState</summary><br />
Request IRacingPlugin to send Session State.

| Name            | Type    | Description                                 |
|:----------------|:-------:|:--------------------------------------------|
| EventType       | string  | `IRacingCommandSendSessionState` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                            |

**JSON Example:**
`{"EventType":"IRacingCommandSendSessionState","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendTrackInfo</summary><br />
Request IRacingPlugin to send Track Info.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingCommandSendTrackInfo` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                         |

**JSON Example:**
`{"EventType":"IRacingCommandSendTrackInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendWeatherInfo</summary><br />
Request IRacingPlugin to send Weather info.

| Name            | Type    | Description                                |
|:----------------|:-------:|:-------------------------------------------|
| EventType       | string  | `IRacingCommandSendWeatherInfo` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                           |

**JSON Example:**
`{"EventType":"IRacingCommandSendWeatherInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingConnected</summary><br />
Sent when connected to IRacing

| Name            | Type    | Description                   |
|:----------------|:-------:|:------------------------------|
| EventType       | string  | `IRacingConnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)              |

**JSON Example:**
`{"EventType":"IRacingConnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingPractice</summary><br />

| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingPractice` (constant)                                       |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |


**JSON Example:**
`{"EventType":"IRacingPractice","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road"}`
....
</details>

<details><summary>IRacingQualify</summary><br />
| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingQualify` (constant)                                        |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |


**JSON Example:**
`{"EventType":"IRacingQualify","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road"}`
</details>

<details><summary>IRacingRace</summary><br />
| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingRace` (constant)                                           |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |


**JSON Example:**
`{"EventType":"IRacingRace","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road"}`
</details>

<details><summary>IRacingTesting</summary><br />


| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingTesting` (constant)                                        |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |


**JSON Example:**
`{"EventType":"IRacingTesting","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road"}`

</details>

<details><summary>IRacingWarmup</summary><br />
| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingWarmup` (constant)                                         |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |


**JSON Example:**
`{"EventType":"IRacingWarmup","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road"}`
</details>

<details><summary>IRacingDisconnected</summary><br />
Sent when connected to IRacing

| Name            | Type    | Description                      |
|:----------------|:-------:|:---------------------------------|
| EventType       | string  | `IRacingDisconnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                 |

**JSON Example:**
`{"EventType":"IRacingDisconnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingDriverIncident</summary><br />

Sent every time an incident is detected (only for user, not other drivers).

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `IRacingDriverIncident` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| IncidentCount   | int     | Total incident count               |
| IncidentDelta   | int     | Delta incident count               |

**JSON Example:**
TODO
</details>

<details><summary>IRacingPitEnter</summary><br />
Sent when a car enters the pit lane.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingPitEnter` (constant)             |
| ExcludeFromTxrx | boolean | false (constant)                         |
| SessionTime     | float   | Time of event (seconds into the session) |
| CarIdx          | int     | Car Index                                |
| LocalUser       | bool    | Is it our car?                           |

**JSON Example:**
`{"EventType":"IRacingPitEnter","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"CarIdx":6,"LocalUser":false}`
</details>

<details><summary>IRacingPitExit</summary><br />
Sent when a car leaves the pit lane.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingPitExit` (constant)              |
| ExcludeFromTxrx | boolean | false (constant)                         |
| SessionTime     | float   | Time of event (seconds into the session) |
| CarIdx          | int     | Car Index                                |
| LocalUser       | bool    | Is it our car?                           |
| Duration        | double  | Duration of the pitstop                  |

**JSON Example:**
`{"EventType":"IRacingPitExit","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1077.1666748046685,"CarIdx":11,"LocalUser":false,"Duration":10.233333333324026}`
</details>

<details><summary>IRacingPitstopReport</summary><br />
For user, this is sent after a pitshop, showing some data about the pitstop.

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingPitstopReport` (constant)        |
| ExcludeFromTxrx | boolean | false (constant)                         |
| SessionTime     | float   | Time of event (seconds into the session) |
| CarIdx          | int     | Car Index                                |
| TempLFL         | uint    | Tyre temperature: Left Front L           |
| TempLFM         | uint    | Tyre temperature: Left Front M           |
| TempLFR         | uint    | Tyre temperature: Left Front R           |
| TempRFL         | uint    | Tyre temperature: Right Front L          |
| TempRFM         | uint    | Tyre temperature: Right Front M          |
| TempRFR         | uint    | Tyre temperature: Right Front R          |
| TempLRL         | uint    | Tyre temperature: Left Rear L            |
| TempLRM         | uint    | Tyre temperature: Left Rear M            |
| TempLRR         | uint    | Tyre temperature: Left Rear R            |
| TempRRL         | uint    | Tyre temperature: Right Rear L           |
| TempRRM         | uint    | Tyre temperature: Right Rear M           |
| TempRRR         | uint    | Tyre temperature: Right Rear R           |
| WearLFL         | uint    | Tyre wear: Left Front L                  |
| WearLFM         | uint    | Tyre wear: Left Front M                  |
| WearLFR         | uint    | Tyre wear: Left Front R                  |
| WearRFL         | uint    | Tyre wear: Right Front L                 |
| WearRFM         | uint    | Tyre wear: Right Front M                 |
| WearRFR         | uint    | Tyre wear: Right Front R                 |
| WearLRL         | uint    | Tyre wear: Left Rear L                   |
| WearLRM         | uint    | Tyre wear: Left Rear M                   |
| WearLRR         | uint    | Tyre wear: Left Rear R                   |
| WearRRL         | uint    | Tyre wear: Right Front L                 |
| WearRRM         | uint    | Tyre wear: Right Front M                 |
| WearRRR         | uint    | Tyre wear: Right Front R                 |
| Laps            | long    | Number of laps completed during stint    |
| FuelDiff        | float   | Fuel level changes                       |
| Duration        | float   | Stint duration                           |

</details>

<details><summary>IRacingRaceFlags</summary><br />

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `IRacingRaceFlags` (constant)            |
| ExcludeFromTxrx | boolean | false (constant)                         |
| SessionTime     | float   | Time of event (seconds into the session) |
| Black           | bool    |                                          |
| Blue            | bool    |                                          |
| Caution         | bool    |                                          |
| CautionWaving   | bool    |                                          |
| Checkered       | bool    |                                          |
| Crossed         | bool    |                                          |
| Debris          | bool    |                                          |
| Disqualify      | bool    |                                          |
| FiveToGo        | bool    |                                          |
| Furled          | bool    |                                          |
| Green           | bool    |                                          |
| GreenHeld       | bool    |                                          |
| OneLapToGreen   | bool    |                                          |
| RandomWaving    | bool    |                                          |
| Red             | bool    |                                          |
| Repair          | bool    |                                          |
| Servicible      | bool    |                                          |
| StartGo         | bool    |                                          |
| StartHidden     | bool    |                                          |
| StartReady      | bool    |                                          |
| StartSet        | bool    |                                          |
| TenToGo         | bool    |                                          |
| White           | bool    |                                          |
| Yellow          | bool    |                                          |
| YellowWaving    | bool    |                                          |

**JSON Example:**
`{"EventType":"IRacingRaceFlags","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"Black":false,"Blue":false,"Caution":false,"CautionWaving":false,"Checkered":false,"Crossed":false,"Debris":false,"Disqualify":false,"FiveToGo":false,"Furled":false,"Green":false,"GreenHeld":false,"OneLapToGreen":false,"RandomWaving":false,"Red":false,"Repair":false,"Servicible":false,"StartGo":false,"StartHidden":true,"StartReady":false,"StartSet":false,"TenToGo":false,"White":false,"Yellow":false,"YellowWaving":false}`
</details>

<details><summary>IRacingTrackInfo</summary><br />

| Name                  | Type    | Description                   |
|:----------------------|:-------:|:------------------------------|
| EventType             | string  | `IRacingTrackInfo` (constant) |
| ExcludeFromTxrx       | boolean | false (constant)              |
| TrackId               | string  |                               |
| TrackLength           | string  |                               |
| TrackDisplayName      | string  |                               |
| TrackCity             | string  |                               |
| TrackCountry          | string  |                               |
| TrackDisplayShortName | string  |                               |
| TrackConfigName       | string  |                               |
| TrackType             | string  |                               |

**JSON Example:**
`{"EventType":"IRacingTrackInfo","ExcludeFromTxrx":false, "Uptime":1742,"TrackId":9,"TrackLength":"3.20 km","TrackDisplayName":"Summit Point Raceway","TrackCity":"Summit Point","TrackCountry":"USA","TrackDisplayShortName":"Summit","TrackConfigName":null,"TrackType":"road course"}`
</details>

<details><summary>IRacingWeatherInfo</summary><br />

| Name             | Type    | Description                     |
|:-----------------|:-------:|:--------------------------------|
| EventType        | string  | `IRacingWeatherInfo` (constant) |
| ExcludeFromTxrx  | boolean | false (constant)                |
| SessionTime      | string  |                                 |
| Skies            | string  |                                 |
| SurfaceTemp      | string  |                                 |
| AirTemp          | string  |                                 |
| AirPressure      | string  |                                 |
| RelativeHumidity | string  |                                 |
| FogLevel         | string  |                                 |

**JSON Example:**
`{"EventType":"IRacingWeatherInfo","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"Skies":"Partly Cloudy","SurfaceTemp":"39.76 C","AirTemp":"25.51 C","AirPressure":"29.25 Hg","RelativeHumidity":"55 %","FogLevel":"0 %"}`
</details>

## Playback

Enables saving and replaying of events. Slipstream stores up-to 10.000 
events that you may save into a plain text file (that you can, and 
should edit). After doing so, you can load these into slipstream again.

<details><summary>PlaybackCommandInjectEvents</summary><br />

Requests that a filename is read and sent as events.


| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Filename         | string  | Filename to read events from|

**JSON Example:**
`{"EventType":"PlaybackCommandInjectEvents","ExcludeFromTxrx":true,"Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T17.53.52.mjson","Uptime":4818}`
</details>

<details><summary>PlaybackCommandSaveEvents</summary><br />

Requests that events already seen, is stored in a file.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Filename         | string  | Filename write events to|

**JSON Example:**
`{"EventType":"PlaybackSaveEvents","ExcludeFromTxrx":true,"Uptime":73571,"Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T21.28.15.mjson"}`
</details>

## Twitch

<details><summary>TwitchCommandSendMessage</summary><br />

Sends a message to the connected twitch channel.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Message         | string  | Message                               |

**JSON Example:**
`{"EventType":"TwitchCommandSendMessage","ExcludeFromTxrx":false, "Uptime":1742,"Message":"Hello"}`
</details>

<details><summary>TwitchCommandSendWhisper</summary><br />

Sends a whisper to a user.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendWhisper` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| To              | string  | Message                               |
| Message         | string  | Message                               |

**JSON Example:**
`{"EventType":"TwitchCommandSendWhisper","ExcludeFromTxrx":false, "Uptime":1742,"To":"tntion", "Message":"Hello"}`
</details>

<details><summary>TwitchConnected</summary><br />
Published when we're connected to Twitch.

| Name            | Type    | Description                  |
|:----------------|:-------:|:-----------------------------|
| EventType       | string  | `TwitchConnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)             |

**JSON Example:**
`{"EventType":"TwitchConnected","ExcludeFromTxrx":false}`
</details>

<details><summary>TwitchDisconnected</summary><br />
We were disconnected from Twitch

| Name            | Type    | Description                     |
|:----------------|:-------:|:--------------------------------|
| EventType       | string  | `TwitchDisconnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                |

**JSON Example:**
`{"EventType":"TwitchConnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>TwitchReceivedMessage</summary><br />

A message received in the channel

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `TwitchReceivedMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| From            | string  | Sender's username                  |
| Message         | boolean | Message typed, including `!`       |
| Moderator       | boolean | Is sender a (twitch) moderator?    |
| Subscriber      | boolean | Is sender a (twitch) subscriber?   |
| Vip             | boolean | Is sender a (twitch) VIP?          |
| Broadcaster     | boolean | Is sender a (twitch) broadcaster?  |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","ExcludeFromTxrx":false, "Uptime":1742,"From":"TNTion","Message":"!hello","Moderator":false,"Subscriber":false,"Vip":false,"Broadcaster":true}`
</details>

<details><summary>TwitchReceivedWhisper</summary><br />

A whisper received.

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `TwitchReceivedWhisper` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| From            | string  | Sender's username                  |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","ExcludeFromTxrx":false, "Uptime":1742,"From":"TNTion","Message":"!hello"}`
</details>

<details><summary>TwitchUserSubscribed</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                 |
|:-----------------|:-------:|:--------------------------------------------|
| EventType        | string  | `TwitchUserSubscribed` (constant)           |
| ExcludeFromTxrx  | boolean | false (constant)                            |
| Name             | string  | User (re)subscriping                        |
| Message          | string  | User own message                            |
| SystemMessage    | string  | Twitch's  message                           |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3` |
| Months           | long    | Subscription length in months               |
</details>

<details><summary>TwitchGiftedSubscription</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                 |
|:-----------------|:-------:|:--------------------------------------------|
| EventType        | string  | `TwitchGiftedSubscription` (constant)       |
| ExcludeFromTxrx  | boolean | false (constant)                            |
| Gifter           | string  | User gifting sub (might be Anonymous)       |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3` |
| SystemMessage    | string  | Twitch's  message                           |
| Recipient        | string  | Gift recipient                              |
</details>

<details><summary>TwitchRaided</summary><br />

A user subscribed or resubscribed to the stream

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `TwitchRaided` (constant)            |
| ExcludeFromTxrx | boolean | false (constant)                     |
| Name            | string  | Who is raiding                       |
| ViewerCount     | int     | How many viewers does the raid bring |
</details>

## UI

<details><summary>UIButtonTriggered</summary><br />
Is sent every time a button is pressed

| Name            | Type    | Description                    |
|:----------------|:-------:|:-------------------------------|
| EventType       | string  | `UIButtonTriggered` (constant) |
| ExcludeFromTxrx | boolean | false (constant)               |
| Text            | string  | Text of the button             |

**JSON Example:**
`{"EventType":"UIButtonTriggered","ExcludeFromTxrx":false, "Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandCreateButton</summary><br />
Create a new button, unless it exists

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `UICommandCreateButton` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| Text            | string  | Text of the button                 |

**JSON Example:**
`{"EventType":"UICommandCreateButton","ExcludeFromTxrx":true, "Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandDeleteButton</summary><br />
Removes a button again, if it exists

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `UICommandDeleteButton` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| Text            | string  | Text of the button                 |

**JSON Example:**
`{"EventType":"UICommandDeleteButton","ExcludeFromTxrx":true, "Uptime":1742,"Text":"World"}`
</details>

<details><summary>UICommandWriteToConsole</summary><br />
Output something to the console.

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `UICommandWriteToConsole` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                      |
| Message         | string  | Message                              |

**JSON Example:**
`{"EventType":"UICommandWriteToConsole","ExcludeFromTxrx":true, "Uptime":1742,"Message":"Hello World"}`
</details>
