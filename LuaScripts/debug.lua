--[[

This script will display all events seen by it to the console. Only for development / debugging purposes.

--]]

function handle(event)
	print(event_to_json(event))
end

print "debug.lua loaded"