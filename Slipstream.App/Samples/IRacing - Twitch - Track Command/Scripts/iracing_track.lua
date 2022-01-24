local cfg = require("lib/config")
local common = require("lib/common")
local iracing = require("api/iracing"):instance(cfg.iracing)
local twitch = require("api/twitch"):instance(cfg.twitch)

local track_name = nil
local track_config = nil
local track_country = nil
local track_city = nil

local track = common.with_cooldown("!track", 30, function()
	if track_name then
		twitch:send_channel_message("We are in " .. track_city .. ", " .. track_country .. " on track " .. track_name .. " " .. (track_config or ""))
	else
		twitch:send_channel_message("I dont know :(")
	end
end)

iracing:send_track_info()

function handle(event)
	if event.EventType == "TwitchReceivedMessage" then
		if string.starts(event.Message, "!track") then
			track()
		end
	elseif event.EventType == "IRacingTrackInfo"  then
		track_name = event.TrackDisplayName
		track_config = event.TrackConfigName
		track_country = event.TrackCountry
		track_city = event.TrackCity
	elseif event.EventType == "IRacingDisconnected" then
		track_name = nil
		track_config = nil
		track_country = nil
		track_city = nil
	end
end