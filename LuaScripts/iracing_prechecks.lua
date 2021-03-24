-- At start of qualify and race, remind user to check their car setup

iracing:send_session_state()

function handle(event)
	if event.EventType == "IRacingQualify" and event.State == "Racing" then
		audio:say("AudioPlugin", "Qualify checklist: Setup and fuel", 1.0)
	end
	if event.EventType == "IRacingRace" and event.State == "GetInCar" then
		audio:say("AudioPlugin", "Race checklist: Setup, tyres, fuel and fast-repairs", 1.0)
	end
end