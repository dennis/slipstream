-- At start of qualify and race, remind user to check their car setup
-- Before using this, edit lib/config.lua

local cfg = require("lib/config")
local audio = require("api/audio"):instance(cfg.audio)
local iracing = require("api/iracing"):instance(cfg.iracing)

iracing:send_session_state()

function handle(event)
	if event.EventType == "IRacingQualify" and event.State == "Racing" then
		audio:say("Qualify checklist: Setup and fuel")
	end
	if event.EventType == "IRacingRace" and event.State == "GetInCar" then
		audio:say("Race checklist: Setup, tyres, fuel and fast-repairs")
	end
end