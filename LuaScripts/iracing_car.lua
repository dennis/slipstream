-- Implements a "!car" command for twitch users to see the current car driven
-- Before using this, edit lib/config.lua

local cfg = require("lib/config")
local common = require("lib/common")
local iracing = require("api/iracing"):instance(cfg.iracing)
local twitch = require("api/twitch"):instance(cfg.twitch)

local car_number = nil
local current_driver = nil
local team_name = nil
local car_name = nil

iracing:send_car_info()

local car = common.with_cooldown("!car", 30, function()
	if car_number then -- we use car_number as a flag to see if data is populated
		if not current_driver == team_name then
			twitch:send_channel_message("We are driving in car #" .. car_number .. " in a " .. car_name .. " for team " .. team_name .. ". " .. current_driver .. " is driving.")
		else
			twitch:send_channel_message("We are driving in car #" .. car_number .. " in a " .. car_name .. ".")
		end
	else
		twitch:send_channel_message("I dont know :(")
	end
end)

function handle(event)
	if event.EventType == "TwitchReceivedMessage" then
		if string.starts(event.Message, "!car") then
			car()
		end
	elseif event.EventType == "IRacingDisconnected" then
		car_name = nil
		current_driver = nil
		team_name = nil
		car_name = nil
	elseif event.EventType == "IRacingCarInfo" and event.LocalUser == true then
		car_number = event.CarNumber
		current_driver = event.CurrentDriverName
		team_name = event.TeamName
		car_name = event.CarName
	end
end
