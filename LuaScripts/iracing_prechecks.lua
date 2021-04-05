-- At start of qualify and race, remind user to check their car setup

local audio = require("api/audio"):instance({ id = "audio" })

iracing:send_session_state()

function handle(event)
	if event.EventType == "IRacingQualify" and event.State == "Racing" then
		audio:say("Qualify checklist: Setup and fuel")
	end
	if event.EventType == "IRacingRace" and event.State == "GetInCar" then
		audio:say("Race checklist: Setup, tyres, fuel and fast-repairs")
	end
end