# Changelog

## Next version
[Full Changelog](https://github.com/dennis/slipstream/compare/v0.2.0...main)

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
