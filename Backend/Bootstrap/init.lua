-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup

local lua = require("api/lua")
local lua_loader = "SlipstreamLuaLoader"

local function ends_with(str, ending)
   return ending == "" or str:sub(-#ending) == ending
end

function handle(e)
	if e.EventType == "FileMonitorFileCreated" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua:instance({id = e.FilePath, file = e.FilePath})
	elseif e.EventType == "FileMonitorFileChanged" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua:instance({id = e.FilePath, file = e.FilePath}):stop()
		wait(e.FilePath, function()
			lua:instance({id = e.FilePath, file = e.FilePath})
		end, 1)
	elseif e.EventType == "FileMonitorFileRenamed" and e.InstanceId == lua_loader then
		if ends_with(e.OldFilePath, ".lua") then lua:instance({id = e.OldFilePath, file = e.OldFilePath}):stop() end
		if ends_with(e.FilePath, ".lua") then lua:instance({id = e.FilePath, file = e.FilePath}) end
	elseif e.EventType == "FileMonitorFileDeleted" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua:instance({id = e.FilePath, file = e.FilePath}):stop()
	elseif e.EventType == "InternalCommandShutdown" then
		lua:instance({id = SS.instance_id, file = SS.file}):stop()
	end
end

require("api/filemonitor"):instance({ id = lua_loader, paths = { "Scripts" } }):scan()
require("api/winformui"):instance({id = "winformui"})
