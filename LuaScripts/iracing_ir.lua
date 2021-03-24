--[[

	Provides "!ir" command, that will shown the latest seen IRating/License info

	- "!ir" - shows IRating/License for current category of the session (road, dirt, dirtoval, dirtroad)
	- "!ir <road|dirt|dirtroad|oval>" - show of another category than current

--]]

local common = require("lib/common")

local session_category = ""
local irating = ""
local license = ""

iracing:send_session_state()
iracing:send_car_info()

local output = function(txt)
	twitch:send_channel_message(txt)
end

local show_irating = function(category)
	category = string.lower(category or "road")

	local irating = state:get("irating:" .. category)
	local license = state:get("license:" .. category)

	if irating ~= "" and license ~= "" then
		output("For " .. category .. " irating is " .. irating .. ", license is " .. license)
	else
		output("I dont know :(")
	end
end

handlers = {}
handlers.IRacingCarInfo = function(event)
	if (irating ~= event.CurrentDriverIRating or license ~= event.CurrentDriverLicense) and event.LocalUser == true and session_category ~= "" then
		irating = event.CurrentDriverIRating
		license = event.CurrentDriverLicense

		ui:print("Saw that we got ir=" .. irating .. " and sr=" .. license)

		state:set("irating:" .. string.lower(session_category), tostring(irating))
		state:set("license:" .. string.lower(session_category), tostring(license))
	end
end

handlers.IRacingPractice = function(event)
	session_category = string.lower(event.Category)
	irating = ""
	license = ""
end

handlers.IRacingQualify = handlers.IRacingPractice
handlers.IRacingRace = handlers.IRacingPractice
handlers.IRacingTesting = handlers.IRacingPractice
handlers.IRacingWarmup = handlers.IRacingPractice

handlers.TwitchReceivedMessage = function(event)
	local msg = string.trim(string.lower(event.Message))

	if msg == "!ir" then
		show_irating(session_category)
	elseif msg == "!ir road" or msg == "!ir oval" or msg == "!ir dirtoval" or msg == "!ir dirtroad" then
		local requested_category = string.sub(msg, 5, string.len(msg))
		show_irating(requested_category)
	else
		output("Use !ir|!ir road|!ir oval|!ir dirtoval|!ir dirtroad")
	end
end

function handle(event)
	if handlers[event.EventType] then
		handlers[event.EventType](event)
	end
end
