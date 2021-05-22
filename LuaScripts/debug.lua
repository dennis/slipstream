--[[

This script will display all events seen by it to the console. Only for development / debugging purposes.

Before using this, edit lib/config.lua

--]]

local cfg = require("lib/config")
local util = require("api/util"):instance(cfg.util)
local ui = require("api/ui"):instance(cfg.ui)

function handle(event)
	if event.EventType ~= "UICommandWriteToConsole" then
		ui:print(util:event_to_json(event))
	end
end

print "debug.lua loaded"