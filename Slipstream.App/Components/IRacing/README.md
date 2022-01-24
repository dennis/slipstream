# IRacing

IRacing support. Events are published as they happen. You can request specific data upon start, 
to be sure your script will see them. 

When IRacingPlugin can fetch data from IRacing it will issue a `IRacingConnected`. Similar when 
connection is lost a `IRacingDisconnected` is sent.

When connected to IRacing you will get a number of events depending on what happens in IRacing.

## Lua

<details><summary>Construction</summary><br />

```lua
local util = require("api/iracing"):instance(config)
```

This will construct an instance of `api/util` or return an existing instance with 
the same `id` if one exists.

`config` is the initial configuration of the instance if one needs to be created. It is a table with one or more keys as defined below.

| Parameter   | Type          | Default    | Description                    |
| :---------- | :-----------: | :--------: | :----------------------------- |
| id          | string        |            | Mandatory: Id of this instance |
| send_raw_state | boolean | Default false. If enabled, sends raw IRacing gamestate |

If you enable `send_raw_state`, you will get an event 60 times a second, containing 
all supported information from IRacing. You can use this to act on changes that isn't 
directly supported. As this causes a significantly load, it is disabled per default.

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


### Pitstop actions

<details><summary>iracing:pit_clear_all()</summary><br />
Clear all pit checkboxes

```lua
iracing:pit_clear_all()
```
</details>

<details><summary>iracing:pit_clear_tyres_change(kpa)</summary><br />
Clear tire pit checkboxes tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_clear_tyres_change(0)
```
</details>

<details><summary>iracing:pit_clear_tyres()</summary><br />
Add fuel

```lua
iracing:pit_clear_tyres()
```
</details

<details><summary>iracing:pit_request_fast_repair()</summary><br />
Request fast repair

```lua
iracing:pit_request_fast_repair()
```
</details>

<details><summary>iracing:pit_add_fuel(liters)</summary><br />
Add fuel

```lua
iracing:pit_add_fuel(30)
```
</details>

<details><summary>iracing:pit_change_left_front_tyre(kpa)</summary><br />
Change the left front tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_change_left_front_tyre(0)
```
</details>

<details><summary>iracing:pit_change_right_front_tyre(kpa)</summary><br />
Change the right front tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_change_right_front_tyre(0)
```
</details>

<details><summary>iracing:pit_change_left_rear_tyre(kpa)</summary><br />
Change the left rear tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_change_left_rear_tyre(0)
```
</details>

<details><summary>iracing:pit_change_right_rear_tyre(kpa)</summary><br />
Change the right rear tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_change_right_rear_tyre(0)
```
</details>

<details><summary>iracing:pit_change_right_rear_tyre(kpa)</summary><br />
Change the right rear tire, optionally specifying the pressure in KPa or pass '0' to use existing pressure

```lua
iracing:pit_change_right_rear_tyre(0)
```
</details>

<details><summary>iracing:pit_clean_windshield()</summary><br />
Clean the winshield, using one tear off

```lua
iracing:pit_clean_windshield()
```
</details>