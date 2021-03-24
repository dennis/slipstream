# IRacing

Provided by IRacingPlugin. Events are published as they happen. You can request specific data upon start, 
to be sure your script will see them. 

When IRacingPlugin can fetch data from IRacing it will issue a `IRacingConnected`. Similar when 
connection is lost a `IRacingDisconnected` is sent.

When connected to IRacing you will get a number of events depending on what happens in IRacing.

## Lua

<details><summary>internal:register_plugin({plugin_name = "IRacingPlugin"})</summary><br />
Registers the IRacingPlugin. 

| Parameter      | Type    | Description                                            |
|:---------------|:-------:|:-------------------------------------------------------|
| plugin_name    | string  | "IRacingPlugin"                                        |
| plugin_id      | string  | Ignored                                                |
| send_raw_state | boolean | Default false. If enabled, sends raw IRacing gamestate |


If you enable `send_raw_state`, you will get an event 60 times a second, containing 
all supported information from IRacing. You can use this to act on changes that isn't 
directly supported. As this causes a significantly load, it is disabled per default.

```lua
internal:register_plugin({plugin_name = "IRacingPlugin", send_raw_estate = true}}
```

The raw state is sent as `IRacingRaw` events.
</details>

<details><summary>iracing:send_car_info()</summary><br />
Request IRacingPlugin to send cars in session.

No arguments

```lua
iracing:send_car_info()
```

This function publishes `IRacingCommandSendCarInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_car_info`` (deprecated)
</details>

<details><summary>iracing:send_track_info()</summary><br />
Request IRacingPlugin to send track information.

No arguments

```lua
iracing:send_track_info()
```

This function publishes `IRacingCommandSendTrackInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_track_info`` (deprecated)
</details>

<details><summary>iracing:send_weather_info()</summary><br />
Request IRacingPlugin to send weather information.

No arguments

```lua
iracing:send_weather_info()
```

This function publishes `IRacingCommandSendWeatherInfo` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_weather_info`` (deprecated)
</details>

<details><summary>iracing:send_session_state()</summary><br />
Request IRacingPlugin to send session state

No arguments

```lua
iracing:send_session_state()
```

This function publishes `IRacingCommandSendSessionState` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_session_state`` (deprecated)
</details>

<details><summary>iracing:send_race_flags()</summary><br />
Request IRacingPlugin to send race flags

No arguments

```lua
iracing:send_race_flags()
```

This function publishes `IRacingCommandSendRaceFlags` event, that is handled by IRacingPlugin.

This function is aliased as ``iracing_send_race_flags`` (deprecated)
</details>

## Events

<details><summary>IRacingCompletedLap</summary><br />

Published every time a driver completes a full lap.

| Name             | Type    | Description                                                                                               |
|:-----------------|:-------:|:----------------------------------------------------------------------------------------------------------|
| EventType        | string  | `IRacingCompletedLap` (constant)                                                                          |
| ExcludeFromTxrx  | boolean | false (constant)                                                                                          |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).                                         |
| SessionTime      | float   | Time of event (seconds into the session)                                                                  |
| CarIdx           | integer | Id of car                                                                                                 |
| LapTime          | float   | Lap time                                                                                                  |
| EstimatedLapTime | boolean | true = if laptime was calculated by Slipstream as no IRacing laptime was available. false=IRacing laptime |
| LapsCompleted    | integer | How many laps were completed                                                                              |
| FuelDelta        | float   | Changes in fuel levels                                                                                    |
| LocalUser        | boolean | Is it our car?                                                                                            |
| BestLap          | boolean | Was this lap a new best lap time in this session?                                                         |

**JSON Example:**
`{"EventType":"IRacingCompletedLap","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":7306.3000976104477,"CarIdx":5,"LapTime":7306.3000976104477,"LapsCompleted":9,"FuelDelta":null,"LocalUser":false,"BestLap":false}`
</details>

<details><summary>IRacingCarInfo</summary><br />
Info about a new car or car with changed details (such as driver).

| Name                 | Type    | Description                                                       |
|:---------------------|:-------:|:------------------------------------------------------------------|
| EventType            | string  | `IRacingCarInfo` (constant)                               |
| ExcludeFromTxrx      | boolean | false (constant)                                                  |
| Uptime               | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime          | float   | Time of event (seconds into the session)                          |
| CarNumber            | string  | Car's number                                                      |
| CurrentDriverUserID  | long    | IRacing Customer Id                                               |
| CurrentDriverName    | string  | Driver's full name                                                |
| TeamID               | long    | IRacing's team Id                                                 |
| TeamName             | string  | IRacing's team name (might be same as Drivers name, if no team)   |
| CarName              | string  | Full name of car                                                  |
| CarNameShort         | string  | Short name of car                                                 |
| CurrentDriverIRating | long    | Drivers IRating                                                   |
| CurrentDriverLicense | string  | Drivers License                                                   |
| LocalUser            | bool    | Is it our car?                                                    |
| Spectator            | bool    | Is car a spectator                                                |

**JSON Example:**
`{"EventType":"IRacingCarInfo","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"CarIdx":63,"CarNumber":"042","CurrentDriverUserID":411093,"CurrentDriverName":"Dennis M\u00F8llegaard Pedersen","TeamID":0,"TeamName":"Dennis M\u00F8llegaard Pedersen","CarName":"Mazda MX-5 Cup","CarNameShort":"MX-5 Cup","CurrentDriverIRating":1592,"CurrentDriverLicense":"A 4.50","LocalUser":true,"Spectator":true}`
</details>

<details><summary>IRacingCommandSendCarInfo</summary><br />

Request IRacingPlugin to send Car Info.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCommandSendCarInfo` (constant)                            |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingCommandSendCarInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendRaceFlags</summary><br />
Request IRacingPlugin to send Race Flags.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCommandSendRaceFlags` (constant)                          |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingCommandSendRaceFlags","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendSessionState</summary><br />
Request IRacingPlugin to send Session State.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCommandSendSessionState` (constant)                       |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingCommandSendSessionState","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendTrackInfo</summary><br />
Request IRacingPlugin to send Track Info.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCommandSendTrackInfo` (constant)                          |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingCommandSendTrackInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingCommandSendWeatherInfo</summary><br />
Request IRacingPlugin to send Weather info.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCommandSendWeatherInfo` (constant)                        |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingCommandSendWeatherInfo","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingConnected</summary><br />
Sent when connected to IRacing

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingConnected` (constant)                                     |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingConnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingPractice</summary><br />

| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingPractice` (constant)                                       |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).  |
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
</details>

<details><summary>IRacingQualify</summary><br />
| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingQualify` (constant)                                        |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).  |
| Category         | string  | `Road`, `Oval`, `DirtOval` or `DirtRoad`                           |
| SessionTime      | float   | Time of event (seconds into the session)                           |
| TimeLimited      | bool    | Is this session time-limited                                       |
| LapsLimited      | bool    | Is this session laps limited                                       |
| TotalSessionLaps | int     | Total session laps                                                 |
| TotalSessionTime | double  | Total session time                                                 |
| State            | string  | Checkered, CoolDown, GetInCar, Invalid, ParadeLaps, Racing, Warmup |
| Category         | string  | Road, Oval, DirtOval, DirtRoad                                     |
| OpenQualify      | bool    | Open or Lone qualify                                               |


**JSON Example:**
`{"EventType":"IRacingQualify","ExcludeFromTxrx":false,"Uptime":1112,"SessionTime":2763.6131673177088,"LapsLimited":false,"TimeLimited":true,"TotalSessionTime":3600.0,"TotalSessionLaps":0,"State":"Racing","Category":"Road",OpenQualify:false}`
</details>

<details><summary>IRacingRace</summary><br />
| Name             | Type    | Description                                                        |
|:-----------------|:-------:|:-------------------------------------------------------------------|
| EventType        | string  | `IRacingRace` (constant)                                           |
| ExcludeFromTxrx  | boolean | false (constant)                                                   |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).  |
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
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).  |
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
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds).  |
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

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingDisconnected` (constant)                                  |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"IRacingDisconnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>IRacingDriverIncident</summary><br />

Sent every time an incident is detected (only for user, not other drivers).

| Name                | Type    | Description                                                       |
|:--------------------|:-------:|:------------------------------------------------------------------|
| EventType           | string  | `IRacingDriverIncident` (constant)                                |
| ExcludeFromTxrx     | boolean | false (constant)                                                  |
| Uptime              | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| DriverIncidentCount | int     | Total incidents for current driver in car                         |
| DriverIncidentDelta | int     | Change since last event                                           |
| TeamIncidentCount   | int     | Total incidents for the whole team                                |
| TeamIncidentDelta   | int     | Change since last event                                           |
| MyIncidentCount     | int     | Total incidents for the user running IRacing locally              |
| MyIncidentDelta     | int     | Change since last event                                           |

**JSON Example:**
`{"EventType":"IRacingDriverIncident","ExcludeFromTxrx":false,"Uptime":262234,"DriverIncidentCount":1,"DriverIncidentDelta":1,"TeamIncidentCount":9,"TeamIncidentDelta":5,"MyIncidentCount":0,"MyIncidentDelta":0}`
</details>

<details><summary>IRacingPitEnter</summary><br />
Sent when a car enters the pit lane.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingPitEnter` (constant)                                      |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime     | float   | Time of event (seconds into the session)                          |
| CarIdx          | int     | Car Index                                                         |
| LocalUser       | bool    | Is it our car?                                                    |

**JSON Example:**
`{"EventType":"IRacingPitEnter","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"CarIdx":6,"LocalUser":false}`
</details>

<details><summary>IRacingPitExit</summary><br />
Sent when a car leaves the pit lane.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingPitExit` (constant)                                       |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime     | float   | Time of event (seconds into the session)                          |
| CarIdx          | int     | Car Index                                                         |
| LocalUser       | bool    | Is it our car?                                                    |
| Duration        | double  | Duration of the pitstop                                           |

**JSON Example:**
`{"EventType":"IRacingPitExit","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1077.1666748046685,"CarIdx":11,"LocalUser":false,"Duration":10.233333333324026}`
</details>

<details><summary>IRacingPitstopReport</summary><br />
For user, this is sent after a pitshop, showing some data about the pitstop.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingPitstopReport` (constant)                                 |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime     | float   | Time of event (seconds into the session)                          |
| CarIdx          | int     | Car Index                                                         |
| TempLFL         | uint    | Tyre temperature: Left Front L                                    |
| TempLFM         | uint    | Tyre temperature: Left Front M                                    |
| TempLFR         | uint    | Tyre temperature: Left Front R                                    |
| TempRFL         | uint    | Tyre temperature: Right Front L                                   |
| TempRFM         | uint    | Tyre temperature: Right Front M                                   |
| TempRFR         | uint    | Tyre temperature: Right Front R                                   |
| TempLRL         | uint    | Tyre temperature: Left Rear L                                     |
| TempLRM         | uint    | Tyre temperature: Left Rear M                                     |
| TempLRR         | uint    | Tyre temperature: Left Rear R                                     |
| TempRRL         | uint    | Tyre temperature: Right Rear L                                    |
| TempRRM         | uint    | Tyre temperature: Right Rear M                                    |
| TempRRR         | uint    | Tyre temperature: Right Rear R                                    |
| WearLFL         | uint    | Tyre wear: Left Front L                                           |
| WearLFM         | uint    | Tyre wear: Left Front M                                           |
| WearLFR         | uint    | Tyre wear: Left Front R                                           |
| WearRFL         | uint    | Tyre wear: Right Front L                                          |
| WearRFM         | uint    | Tyre wear: Right Front M                                          |
| WearRFR         | uint    | Tyre wear: Right Front R                                          |
| WearLRL         | uint    | Tyre wear: Left Rear L                                            |
| WearLRM         | uint    | Tyre wear: Left Rear M                                            |
| WearLRR         | uint    | Tyre wear: Left Rear R                                            |
| WearRRL         | uint    | Tyre wear: Right Front L                                          |
| WearRRM         | uint    | Tyre wear: Right Front M                                          |
| WearRRR         | uint    | Tyre wear: Right Front R                                          |
| Laps            | long    | Number of laps completed during stint                             |
| FuelDelta       | float   | Fuel level changes                                                |
| Duration        | float   | Stint duration                                                    |

</details>

<details><summary>IRacingRaceFlags</summary><br />

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingRaceFlags` (constant)                                     |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime     | float   | Time of event (seconds into the session)                          |
| Black           | bool    |                                                                   |
| Blue            | bool    |                                                                   |
| Caution         | bool    |                                                                   |
| CautionWaving   | bool    |                                                                   |
| Checkered       | bool    |                                                                   |
| Crossed         | bool    |                                                                   |
| Debris          | bool    |                                                                   |
| Disqualify      | bool    |                                                                   |
| FiveToGo        | bool    |                                                                   |
| Furled          | bool    |                                                                   |
| Green           | bool    |                                                                   |
| GreenHeld       | bool    |                                                                   |
| OneLapToGreen   | bool    |                                                                   |
| RandomWaving    | bool    |                                                                   |
| Red             | bool    |                                                                   |
| Repair          | bool    |                                                                   |
| Servicible      | bool    |                                                                   |
| StartGo         | bool    |                                                                   |
| StartHidden     | bool    |                                                                   |
| StartReady      | bool    |                                                                   |
| StartSet        | bool    |                                                                   |
| TenToGo         | bool    |                                                                   |
| White           | bool    |                                                                   |
| Yellow          | bool    |                                                                   |
| YellowWaving    | bool    |                                                                   |

**JSON Example:**
`{"EventType":"IRacingRaceFlags","ExcludeFromTxrx":false, "Uptime":1742,"SessionTime":1058.3000081380189,"Black":false,"Blue":false,"Caution":false,"CautionWaving":false,"Checkered":false,"Crossed":false,"Debris":false,"Disqualify":false,"FiveToGo":false,"Furled":false,"Green":false,"GreenHeld":false,"OneLapToGreen":false,"RandomWaving":false,"Red":false,"Repair":false,"Servicible":false,"StartGo":false,"StartHidden":true,"StartReady":false,"StartSet":false,"TenToGo":false,"White":false,"Yellow":false,"YellowWaving":false}`
</details>

<details><summary>IRacingRaw</summary><br />

| Name            | Type                                                     | Description                                                       |
|:----------------|:--------------------------------------------------------:|:------------------------------------------------------------------|
| EventType       | string                                                   | `IRacingRaw` (constant)                                           |
| ExcludeFromTxrx | boolean                                                  | false (constant)                                                  |
| Uptime          | integer                                                  | Time of when the message was sent via Eventbus (in milliseconds). |
| CurrentState    | [IState](Components/IRacing/Plugins/GameState/IState.cs) | Object containing Slipstreams IRacing state                       |

`IState` is not documented. Please refer to the sourcecode.
</details>

<details><summary>IRacingTrackInfo</summary><br />

| Name                  | Type    | Description                                                       |
|:----------------------|:-------:|:------------------------------------------------------------------|
| EventType             | string  | `IRacingTrackInfo` (constant)                                     |
| ExcludeFromTxrx       | boolean | false (constant)                                                  |
| Uptime                | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| TrackId               | string  |                                                                   |
| TrackLength           | string  |                                                                   |
| TrackDisplayName      | string  |                                                                   |
| TrackCity             | string  |                                                                   |
| TrackCountry          | string  |                                                                   |
| TrackDisplayShortName | string  |                                                                   |
| TrackConfigName       | string  |                                                                   |
| TrackType             | string  |                                                                   |

**JSON Example:**
`{"EventType":"IRacingTrackInfo","ExcludeFromTxrx":false, "Uptime":1742,"TrackId":9,"TrackLength":"3.20 km","TrackDisplayName":"Summit Point Raceway","TrackCity":"Summit Point","TrackCountry":"USA","TrackDisplayShortName":"Summit","TrackConfigName":null,"TrackType":"road course"}`
</details>

<details><summary>IRacingWeatherInfo</summary><br />

| Name             | Type    | Description                                                       |
|:-----------------|:-------:|:------------------------------------------------------------------|
| EventType        | string  | `IRacingWeatherInfo` (constant)                                   |
| ExcludeFromTxrx  | boolean | false (constant)                                                  |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime      | float   |                                                                   |
| Skies            | string  | "Clear", "PartlyCloudy", "MostlyCloudy", "Overcast"               |
| SurfaceTemp      | float   |                                                                   |
| AirTemp          | float   |                                                                   |
| AirPressure      | float   |                                                                   |
| RelativeHumidity | float   |                                                                   |
| FogLevel         | float   |                                                                   |

**JSON Example:**
`{"EventType":"IRacingWeatherInfo","ExcludeFromTxrx":false,"Uptime":549,"SessionTime":1911.8300048828126,"Skies":"PartlyCloudy","SurfaceTemp":31.1111145,"AirTemp":25.5555553,"AirPressure":29.92,"RelativeHumidity":0.55,"FogLevel":0.0}`
</details>

<details><summary>IRacingCarPosition</summary><br />

Published every time car changes positions.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `IRacingCarPosition` (constant)                                   |
| ExcludeFromTxrx | boolean | false (constant)                                                  |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime     | float   |                                                                   |
| CarIdx          | int     | Car Index                                                         |
| LocalUser       | bool    | Is it our car?                                                    |
| PositionInClass | int     | Position in class                                                 |
| PositionInRace  | int     | Overall position in race                                          |

**JSON Example:**
`{"EventType":"IRacingCarPosition","ExcludeFromTxrx":false,"Uptime":2528216,"SessionTime":2711.7666666666669,"CarIdx":28,"LocalUser":true,"PositionInClass":15,"PositionInRace":15}`
</details>

<details><summary>IRacingTowed</summary><br />

Published every your car is being towed to pits. You will not get these events for other cars. These isn't published during replays.

| Name             | Type    | Description                                                       |
|:-----------------|:-------:|:------------------------------------------------------------------|
| EventType        | string  | `IRacingTowed` (constant)                                         |
| ExcludeFromTxrx  | boolean | false (constant)                                                  |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime      | float   |                                                                   |
| RemainingTowTime | float   | How many seconds before we're in pit                              |

**JSON Example:**
`{"EventType":"IRacingTowed","ExcludeFromTxrx":false,"Uptime":2528216,"SessionTime":2711.7666666666669,"CarIdx":28,"LocalUser":true,"PositionInClass":15,"PositionInRace":15}`
</details>

<details><summary>IRacingTrackPosition</summary><br />

Published every time a car changes position (overtakes or get overtaken). Mostly, there
will only be on other car in the event (in `NewCarsAhead` or `NewCarsBehind`)

| Name                    | Type     | Description                                                       |
|:------------------------|:--------:|:------------------------------------------------------------------|
| EventType               | string   | `IRacingTrackPosition` (constant)                                 |
| ExcludeFromTxrx         | boolean  | false (constant)                                                  |
| Uptime                  | integer  | Time of when the message was sent via Eventbus (in milliseconds). |
| SessionTime             | float    |                                                                   |
| CarIdx                  | int      | Car Index                                                         |
| LocalUser               | boolean  | Is it our car?                                                    |
| CurrentPositionInClass  | int      | Our current position in our class.                                |
| CurrentPositionInRace   | int      | Our current position in race.                                     |
| PreviousPositionInClass | int      | Our current position in our class.                                |
| PreviousPositionInRace  | int      | Our current position in race.                                     |
| NewCarsAhead            | [CarIdx] | Array of new CarIdx that overtook the car                         |
| NewCarsBehind           | [CarIdx] | Array of new CarIdx that car overtook                             |
**JSON Example:**
`{"EventType":"IRacingTrackPosition","ExcludeFromTxrx":false,"Uptime":63990,"SessionTime":360.383321126302,"CarIdx":39,"LocalUser":true,"CurrentPositionInClass":35,"CurrentPositionInRace":35,"PreviousPositionInClass":34,"PreviousPositionInRace":34,"NewCarsAhead":[7],"NewCarsBehind":[]}`
</details>


