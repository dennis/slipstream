local unpack = unpack or table.unpack

common = {}

common.file_exist = function(filename)
	local f = io.open(filename, "r") -- a bit naive, as it only checks if the file is readable
	
	if f~=nil then 
		io.close(f)
		return true
	else
		return false
	end
end

common.write = function(filename, content)
	local f = io.open(filename, "a+")
	io.output(f)
	io.write(content)
	io.close(f)
end

function string.starts(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end

function string.trim(s)
   return s:gsub("^%s+", ""):gsub("%s+$", "")
end

common.with_cooldown = function(name, duration, f)
	local disabled = false

	local enabler = function()
		disabled = false
	end

	return function(...)
		local tp = {...}
		if not disabled then
			f(unpack(tp))

			disabled = true

			core:wait(name, enabler, duration);
		end
	end
end

local datetime_format = "%Y-%m-%d %H:%M:%S"

common.now = function()
	local timestamp = os.time();
	return os.date(datetime_format, timestamp)
end

common.seconds_to_time = function(seconds)
	local h = math.floor(seconds/3600)
	seconds = seconds - h*3600
	local m = math.floor(seconds/60)
	seconds = seconds -  m*60

	if h > 0 then
		return string.format("%d:%02.f:%06.03f", h, m, seconds);
	else
	    return string.format("%02.f:%06.03f", m, seconds);
	end
end

return common