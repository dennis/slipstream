-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup

local lua_loader = "SlipstreamLuaLoader"
local lua = require("api/lua")

local function ends_with(str, ending)
	return ending == "" or str:sub(-#ending) == ending
end

local lua_scripts = {
	scripts = {},

	start = function(self, file)
		if not self.scripts[file] then
			self.scripts[file] = lua:instance({id = file, file = file})
			self.scripts[file]:start()
		else
			self.scripts[file]:restart()
		end
	end,

	stop = function(self, file)
		if self.scripts[file] then
			self.scripts[file]:stop()
		end
	end
}

function handle(e)
	print(e.EventType)
	if e.EventType == "FileMonitorFileCreated" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua_scripts:start(e.FilePath)
	elseif e.EventType == "FileMonitorFileChanged" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua_scripts:start(e.FilePath)
	elseif e.EventType == "FileMonitorFileRenamed" and e.InstanceId == lua_loader then
		if ends_with(e.OldFilePath, ".lua") then
			lua_scripts:stop(e.OldFilePath)
		end
		if ends_with(e.FilePath, ".lua") then
			lua_scripts:start(e.FilePath)
		end
	elseif e.EventType == "FileMonitorFileDeleted" and e.InstanceId == lua_loader and ends_with(e.FilePath, ".lua") then
		lua_scripts:stop(e.FilePath)
	elseif e.EventType == "InternalCommandShutdown" then
		lua:instance({id = SS.instance_id, file = SS.file}):stop()
	end
end

require("api/filemonitor"):instance({ id = lua_loader, paths = { "Scripts" } }):scan()
require("api/winformui"):instance({id = "winformui"})
