--[[

This script will display all events seen by it to the console. Only for development / debugging purposes

--]]

local handles = {}

--
-- Internal
--

handles.CommandPluginDisable = function(event) print(event.EventType .. " Id=" .. event.Id) end
handles.CommandPluginEnable = function(event) print(event.EventType .. " Id=" .. event.Id) end
handles.CommandPluginRegister = function(event) print(event.EventType .. " Id=" .. event.Id .. ", PluginName=" .. event.PluginName) end
handles.CommandPluginStates = function(event) print(event.EventType) end
handles.CommandPluginUnregister = function(event) print(event.EventType .. " Id=" .. event.Id) end
handles.FileMonitorFileChanged = function(event) print(event.EventType .. " FilePath=" .. (event.FilePath or "<NULL>")) end
handles.FileMonitorFileCreated = function(event) print(event.EventType .. " FilePath=" .. (event.FilePath or "<NULL>")) end
handles.FileMonitorFileChanged = function(event) print(event.EventType .. " FilePath=" .. (event.FilePath or "<NULL>")) end
handles.FileMonitorFileRenamed = function(event) print(event.EventType .. " FilePath=" .. (event.FilePath or "<NULL>") .. ", OldFilePath=" .. (event.OldFilePath or "<NULL>")) end
handles.PluginsReady = function(event) print(event.EventType) end
handles.PluginState = function(event) print(event.EventType .. " Id=" .. event.Id  .. ", DisplayName=" .. event.DisplayName .. ", PluginName=" .. event.PluginName .. ", PluginStatus=" .. event.PluginStatus) end

--
-- IRacing
--

handles.IRacingCarCompletedLap = function(event)
    print(event.EventType 
        .. "SessionTime=" .. CarIdx.SessionTime 
        .. ", CarIdx=" .. event.CarIdx 
        .. ", LapsComplete=" .. event.LapsComplete 
        .. ", FuelDiff=" .. (event.FuelDiff or "N/A") 
        .. ", LocalUser=" .. event.LocalUser
    )
end

handles.IRacingCarInfo = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", CarIdx=" .. event.CarIdx
        .. ", CarNumber=" .. event.CarNumber
        .. ", CurrentDriverUserID=" .. event.CurrentDriverUserID
        .. ", CurrentDriverName=" .. event.CurrentDriverName
        .. ", TeamID=" .. event.TeamID
        .. ", TeamName=" .. event.TeamName
        .. ", CarName=" .. event.CarName
        .. ", CarNameShort=" .. event.CarNameShort
        .. ", CurrentDriverIRating=" .. event.CurrentDriverIRating
        .. ", LocalUser=" .. event.LocalUser
        .. ", Spectator=" .. event.Spectator
	)
end

handles.IRacingConnected = function(event) print(event.EventType) end

handles.IRacingCurrentSession = function(event)
	print(event.EventType 
        .. " SessionType=" .. event.SessionType
        .. ", TimeLimited=" .. event.TimeLimited
        .. ", LapsLimited=" .. event.LapsLimited
        .. ", TotalSessionLaps=" .. event.TotalSessionLaps
        .. ", TotalSessionTime=" .. event.TotalSessionTime
    )
end

handles.IRacingDisconnected = function(event) print(event.EventType) end

handles.IRacingPitEnter = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", CarIdx=" .. event.CarIdx
        .. ", LocalUser=" .. event.LocalUser
    )
end

handles.IRacingPitExit = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", CarIdx=" .. event.CarIdx
        .. ", LocalUser=" .. event.LocalUser
        .. ", Duration=" .. (event.Duration or "N/A")
    )
end

handles.IRacingPitstopReport = function(event)
	print("Tyres after " .. event.Laps .. ":")
    print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", CarIdx=" .. event.CarIdx
        .. ", Laps=" .. event.Laps
        .. ", FuelDiff=" .. event.FuelDiff
        .. ", Duration=" .. event.Duration
    )
	print(" LF " .. (event.TempLFL or "N/A") .. "/" .. (event.TempLFM or "N/A") .. "/" .. (event.TempLFR or "N/A"))
	print(" -  " .. (event.WearLFL or "N/A") .. "/" .. (event.WearLFM or "N/A") .. "/" .. (event.WearLFR or "N/A"))
	print(" RF " .. (event.TempRFL or "N/A") .. "/" .. (event.TempRFM or "N/A") .. "/" .. (event.TempRFR or "N/A"))
	print(" -  " .. (event.WearRFL or "N/A") .. "/" .. (event.WearRFM or "N/A") .. "/" .. (event.WearRFR or "N/A"))

	print(" LR " .. (event.TempLRL or "N/A") .. "/" .. (event.TempLRM or "N/A") .. "/" .. (event.TempLRR or "N/A"))
	print(" -  " .. (event.WearLRL or "N/A") .. "/" .. (event.WearLRM or "N/A") .. "/" .. (event.WearLRR or "N/A"))
	print(" RR " .. (event.TempRRL or "N/A") .. "/" .. (event.TempRRM or "N/A") .. "/" .. (event.TempRRR or "N/A"))
	print(" -  " .. (event.WearRRL or "N/A") .. "/" .. (event.WearRRM or "N/A") .. "/" .. (event.WearRRR or "N/A"))
end

handles.IRacingRaceFlags = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", Black=" .. event.Black
        .. ", Blue=" .. event.Blue
        .. ", Caution=" .. event.Caution
        .. ", CautionWaving=" .. event.CautionWaving
        .. ", Checkered=" .. event.Checkered
        .. ", Crossed=" .. event.Crossed
        .. ", Debris=" .. event.Debris
        .. ", Disqualify=" .. event.Disqualify
        .. ", FiveToGo=" .. event.FiveToGo
        .. ", Furled=" .. event.Furled
        .. ", Green=" .. event.Green
        .. ", GreenHeld=" .. event.GreenHeld
        .. ", OneLapToGreen=" .. event.OneLapToGreen
        .. ", RandomWaving=" .. event.RandomWaving
        .. ", Red=" .. event.Red
        .. ", Repair=" .. event.Repair
        .. ", Servicible=" .. event.Servicible
        .. ", StartGo=" .. event.StartGo
        .. ", StartHidden=" .. event.StartHidden
        .. ", StartReady=" .. event.StartReady
        .. ", StartSet=" .. event.StartSet
        .. ", TenToGo=" .. event.TenToGo
        .. ", White=" .. event.White
        .. ", Yellow=" .. event.Yellow
        .. ", YellowWaving=" .. event.YellowWaving
    )
end

handles.IRacingSessionState = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", State=" .. event.State
    )
end

handles.IRacingTrackInfo = function(event)
	print(event.EventType 
        .. " TrackId=" .. event.TrackId
        .. ", TrackLength=" .. event.TrackLength
        .. ", TrackDisplayName=" .. event.TrackDisplayName
        .. ", TrackCity=" .. event.TrackCity
        .. ", TrackCountry=" .. event.TrackCountry
        .. ", TrackDisplayShortName=" .. event.TrackDisplayShortName
        .. ", TrackConfigName=" .. event.TrackConfigName
        .. ", TrackType=" .. event.TrackType
    )
end

handles.IRacingWeatherInfo = function(event)
	print(event.EventType 
        .. " SessionTime=" .. event.SessionTime
        .. ", Skies=" .. event.Skies
        .. ", SurfaceTemp=" .. event.SurfaceTemp
        .. ", AirTemp=" .. event.AirTemp
        .. ", AirPressure=" .. event.AirPressure
        .. ", RelativeHumidity=" .. event.RelativeHumidity
        .. ", FogLevel=" .. event.FogLevel
    )
end

--
-- Twitch
--

handles.CommandTwitchSendMessage = function(event) print(event.EventType .. " Message=" .. event.Message) end
handles.TwitchConnected = function(event) print(event.EventType) end
handles.TwitchDisconnected = function(event) print(event.EventType) end
handles.TwitchReceivedCommand = function(event)
	print(event.EventType 
        .. " From=" .. event.From
        .. ", Message=" .. event.Message
        .. ", Moderator=" .. event.Moderator
        .. ", Subscriber=" .. event.Subscriber
        .. ", Vip=" .. event.Vip
        .. ", Broadcaster=" .. event.Broadcaster
    )
end

--
-- Utilities
--

handles.CommandPlayAudio = function(event)
	print(event.EventType 
        .. " Filename=" .. (event.Filename or "N/A")
        .. ", Volume=" .. (event.Volume or "N/A")
    )
end

handles.CommandSay = function(event)
	print(event.EventType 
        .. " Message=" .. (event.Message or "N/A")
        .. ", Volume=" .. (event.Volume or "N/A")
    )
end

handles.CommandWriteToConsole = function(event)
	print(event.EventType 
        .. " Message=" .. (event.Message or "N/A")
    )
end

function handle(event)
	local cb = handles[event.EventType]

	if cb then
		cb(event)
	else
		print(event.EventType)
	end
end

print "debug.lua loaded"