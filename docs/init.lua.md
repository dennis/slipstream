# init.lua

The init.lua file is the only required Lua script to run the slipstream application.
The file will be created if it does not exist.

Located in %AppData%/Slipstream/init-{app-version}.lua

IMPORTANT: init.lua is application version specific and a new default will be generated
on startup of a different version. This is done to prioritise slipstream startup instead
of Lua script compatability during the high churn phases of development and will be looked
at again once plugin development slows down.

## Usage
init.lua can be used to customise the loading of plugins and change the default behaviour of
a plugin. Make sure to look at individual plugins for more information on registration options.

### Default init.lua
```lua

-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup
print "Initializing"

-- Listens for samples to play or text to speek. Disabling this will mute all
-- sounds
register_plugin("AudioPlugin", "AudioPlugin")

-- Delivers IRacing events as they happen
register_plugin("IRacingPlugin", "IRacingPlugin")

-- Connects to Twitch (via the values provided in Settings) and provide
-- a way to sende and receive twitch messages
register_plugin("TwitchPlugin", "TwitchPlugin")

-- Only one of these may be active at a time. ReceiverPlugin listens
-- for TCP connections, while Transmitter will send the events it sees
-- to the destination. Both are configured as Txrx in Settings.
-- register_plugin("TransmitterPlugin", "TransmitterPlugin")
-- register_plugin("ReceiverPlugin", "ReceiverPlugin")

-- LuaManagerPlugin listens for FileMonitorPlugin events and acts on them.
-- It will only act on files ending with .lua, which it launches
-- a LuaPlugin for. If the file is modified, it will take down the plugin and
-- launch a new one with the same file. If files are moved out of the directory
-- it is consider as if it were deleted. Deleted files are taken down.
register_plugin("LuaManagerPlugin", "LuaManagerPlugin")

-- FileMonitorPlugin monitors the script directory and sends out events
-- every time a file is created, renamed, modified or deleted
register_plugin("FileMonitorPlugin", "FileMonitorPlugin")

```