-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup
print "Initializing"

-- Monitors for new versions of the application and raise events for it
register_plugin({plugin_name = "ApplicationUpdatePlugin", updateLocation="https://github.com/dennis/slipstream", prerelease=true})

-- Delivers IRacing events as they happen
-- register_plugin({ plugin_name = "IRacingPlugin"})

--Connects to Twitch(via the values provided in Settings) and provide
--a way to sende and receive twitch messages.Generate a token here: https://twitchapps.com/tmi/
--register_plugin({ plugin_name = "TwitchPlugin", twitch_username = "<username>", twitch_token = "<token>", twitch_channel = "<channel>"})

--Only one of these may be active at a time.ReceiverPlugin listens
-- for TCP connections, while Transmitter will send the events it sees
--to the destination. Both are configured as Txrx in Settings.
-- register_plugin({ plugin_name = "TransmitterPlugin", ip = " < yourip > ", port = < yourport >})
--register_plugin({ plugin_name = "ReceiverPlugin", ip = " < yourip > ", port = < yourport >})

--LuaManagerPlugin listens for FileMonitorPlugin events and acts on them.
-- It will only act on files ending with.lua, which it launches
-- a LuaPlugin for. If the file is modified, it will take down the plugin and
-- launch a new one with the same file.If files are moved out of the directory
-- it is consider as if it were deleted. Deleted files are taken down.
register_plugin({ plugin_name = "LuaManagerPlugin"})

--FileMonitorPlugin monitors the script directory and sends out events
-- every time a file is created, renamed, modified or deleted
register_plugin({ plugin_name = "FileMonitorPlugin"})

--Provides save / replay of events. Please be careful if you use this.There is
 --not much filtering, so RegisterPlugin / Unregister plugins will actually make
--slipstream perform these actions
register_plugin({ plugin_name = "PlaybackPlugin"})

-- UI to show console output
register_plugin({ plugin_name = "UIPlugin"})
register_plugin({ plugin_name = "WinFormUIPlugin"})