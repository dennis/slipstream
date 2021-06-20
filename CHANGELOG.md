# Changelog

## Next version
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.7.0...main)
 - IRacing: Adds pit-stop lua functions: pit_clear_all(), pit_clear_tyres(), 
   pit_fast_repair(), pit_add_fuel(), pit_change_(left|right)_(front|rear)_tyre()
   and pit_clean_windshield()
 - BUGFIX: Twitch messages would sometimes be dropped
 - Remove persisting the location and size of the Slipstream UI. It could end up
   placing the Window off-screen, making it hard to reach.
 - IRacing: Add FuelLeft attribute to IRacingCompletedLap and IRacingPitExit

## [0.7.0](https://github.com/dennis/slipstream/releases/tag/v0.7.0) (2021-04-30)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.6.0...v0.7.0)
  - BREAKING CHANGE: Rearchitects how Slipstream exposes features to Lua. New init.lua required!
    For more information look at commit 21070e613ac46950b7d1a8465e22f85b5355865f

## [0.6.0](https://github.com/dennis/slipstream/releases/tag/v0.6.0) (2021-04-30)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.5.0...v0.6.0)
  - adds WinFormUI component - containing the Slipstream UI. You need to add `register_plugin({ plugin_name = "WinFormUIPlugin"})
` to you `init.lua` to get it.
  - add `internal:shutdown()` lua function that quits the application
  - Adds `InternalPlugin` (automatically loaded)
  - Adds `UIPlugin` that handles generic UI functionality. Needs to be added to your init.lua!

## [0.5.0](https://github.com/dennis/slipstream/releases/tag/v0.5.0) (2021-03-25)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.4.1...v0.5.0)
  - No more version specific init.lua. From this point and forward, Slipstream only reads init.lua
  - Event: Adds IRacingTrackPosition
  - Adding ApplicationUpdate plugin to manage app install and auto updates using Squirrel.Windows
  - Moved the init.lua default content to a resource

## [0.4.1](https://github.com/dennis/slipstream/releases/tag/v0.4.1) (2021-03-20)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.4.0...v0.4.1)
  - Bugfix: Event IRacingCarInfo now sets LocalUser correctly
  - Event: Added CurrentDriverLicense to IRacingCarInfo
  - Event: Added Category to IRacingCurrentSession
  - Event: Rename LuaManagerCommandDeduplicateEvents to LuaCommandDeduplicateEvents
  - Lua: core:wait('name', .. ) can now redefine itself within the function invoked by wait.
  - lua: Adds audio:send_devices(..), audio:set_output(..) to control which output device to use
  - lua: Changed audio:play(..), audio:say(..) with another arguments to decide which plugin to use
  - events: Adds AudioCommandSendDevices, AudioCommandSetOutputDevice and AudioOutputDevice
  - New plugin: PlaybackPlugin. Allows you to save seen events and inject them again (for debugging and testing)
    - New events: PlaybackCommandInjectEvents (inject events from file), PlaybackCommandSaveEvents (save last events to file)
    - Menu items in UI are only shown if you register that plugin: register_plugin("PlaybackPlugin", "PlaybackPlugin")_
    - Lua: playback:save(filename), playback:load(filename). Saves and loads events (same as via menu in UI)
  - lua: Changed syntax for register_plugin(id, name). Going forward it takes a single argument a lua table. Read [docs](docs/lua.md#internal)
  - UI: Removed settings menu. Now settings is specified in init.lua, making Settings redundant. If you remove your settings, a new one will be generated with instructions.
  - Redesigned events converning the state of the current session
    - Removed events: IRacingCommandSendCurrentSession, IRacingCurrentSession and IRacingSessionState
    - Replaced with: IRacingPractice, IRacingQualify, IRacingRace, IRacingTesting, IRacingWarmup
    - If your script needs to get the current state, it can still use the lua function: iracing:send_session_state()
  - TwitchPlugin: Add events TwitchUserSubscribed (both sub and resub), TwitchGiftedSubscription and TwitchRaided
  - TwitchPlugin: Made it Throttle-aware, hopefully avoiding getting locked out of chat for 30mins
  - IRacingPlugin: Added IRacingCarPosition
  - New plugin: DiscordPlugin
  - IRacingPlugin: Fix IRacingWeatherInfo - now reports actual values and not values from start of session.
  - IRacingPlugin: Support sending "raw" game state events. Allowing you to act on data changes not directly represented as an event.
  - IRacingPlugin: Rename IRacingCarCompletedLap to IRacingCompletedLap
  - IRacingPlugin: Renamed FuelDiff to FuelDelta in IRacingCompletedLap, IRacingPitstopReport for consistency
  - IRacingPlugin: Adds BestLap to IRacingCompletedLap.
  - IRacingPlugin: Rename IRacingCompletedLap.Time to IRacingCompletedLap.LapTime.
  - IRacingPlugin: Add IRacingCompletedLap.EstimatedLapTime - to show if this was a laptime as recorded by IRacing or by Slipstream in case IRacing doesn't provide a laptime
  - IRacingPlugin: Change IRacingDriverIncident: Rename Incident(Count|Delta) to DriverIncident(Count|Delta). Added fields: TeamIncident(Count|Delta) and MyIncident(Count|Delta)
  - IRacingPlugin: Adds: IRacingTowed event

## [0.4.0](https://github.com/dennis/slipstream/releases/tag/v0.4.0) (2020-01-10)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.3.0...v0.4.0)
 - Lua: Deduplicate identical events. If one or more scripts sends the same
   event on the eventbus during start, they will be deduplicated. This means,
   that we avoid having the same response to the same command repeated several
   times. Fx. Having multiple scripts that asks for IRacing WeatherInfo upon
   start, will be deduplicated as only one response is needed for these scripts.
 - Lua: Add iracing_send_car_info() that will make IRacingPlugin send all the CarInfo
 - Lua: Add iracing_send_track_info() that will make IRacingPlugin send all TrackInfo
 - Lua: Add iracing_send_weather_info() that will make IRacingPlugin send all WeatherInfo
 - Lua: Add iracing_send_current_session() that will make IRacingPlugin send CurrentSession
 - Lua: Add iracing_send_session_state() that will make IRacingPlugin send SessionState
 - Lua: Add iracing_send_race_flags() that will make IRacingPlugin send race flags
 - InternalInitialized event removed. Refactored to avoid having it
 - Lua: Removed plugin_enable(), plugin_disable(). Register/Unregister them instead
 - Renamed Events:
   - UICommandWriteToConsole is now UIMethodCollection
   - InternalReconfigured is now InternalCommandReconfigure
   - InternalBootupEvents is now InternalCommandDeduplicateEvents
   - InternalCommandDeduplicateEvents is now LuaManagerCommandDeduplicateEvents
 - Plugin renamed: FileTriggerPlugin to LuaManagerPlugin
 - UI: Stores/restores Window position and size
 - Event: Adds TwitchReceivedMessage event (captures all messages, not only commands)
 - Event: Removes TwitchReceivedCommand event as this is already sent as a TwitchReceivedMessage
 - Event: Adds TwitchReceivedWhisper and TwitchCommandSendWhisper event
 - Lua: send_twitch_whisper() / twitch:send_whisper_message()

## [0.3.0](https://github.com/dennis/slipstream/releases/tag/v0.3.0) (2020-01-05)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.2.0...v0.3.0)

**Improvements**
 - Lua: Add event_to_json() function, that will convert a Slipstream event to a json string. Change debug.lua to use that, making it trivial.
 - Lua: At start: Make sure all Lua script before IRacingPlugin publishes events
 - UI: Will show all events, even if they are published before UI was ready
 - Events renamed for consistency:
   - CommandPlayAudio to AudioCommandPlay
   - CommandSay to AudioCommandSay
   - CommandPluginEnable to InternalCommandPluginEnable
   - CommandPluginDisable to InternalCommandPluginDisable
   - CommandPluginRegister to InternalCommandPluginRegister
   - CommandPluginStates to InternalCommandPluginStates
   - CommandPluginUnregister to InternalCommandPluginUnregister
   - Initialized to InternalInitialized
   - PluginState to InternalPluginState
   - CommandTwitchSendMessage to TwitchCommandSendMessage
   - CommandWriteToConsole to UICommandWriteToConsole
 - Buttons: Make simple buttons in UI using Lua (create_button("text")) and remove them again (delete_button("text"))
 - Lua: Adds functions to control plugins: register_plugin, unregister_plugin, enable_plugin, disable_plugin
 - init-<versionno>.lua is read upon startup. Here you can define what plugins to register and enable. If file is not found, one is created

## [0.2.0](https://github.com/dennis/slipstream/releases/tag/v0.2.0) (2020-12-30)
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.1.0...v0.2.0)

**Improvements**
 - Replace DebugOutputPlugin with a Lua script (LuaScript/debug.lua)
 - Twitch: Allow Twitchchannel to be configured (it was assumed that username and channel would be the same)
 - Added TransmitterPlugin and ReceiverPlugin. Allows one instance of Slipstream to forward its events to another instance using a TCP/IP connection. This features SHOULD ONLY be used locally. There is no authentication or encryption of the events.
 - Twitch: Optionally show twitch log
 - TwitchConnected event are now sent when we joined the channel (not just connected to twitch servers)

**Bugfixes**
 - Auto-create both "Audio" and "Scripts" directories

## [0.1.0](https://github.com/dennis/slipstream/releases/tag/v0.1.0) (2020-12-23)
[Full Changelog](https://github.com/dennis/slipstream/compare/be57351b1d0c5ff75a87ece10b3e7c272a980446...v0.1.0)

**Initial version**
