local cfg = {
	-- This does not need to be changed
	util = { id = "util" },
	winformui = { id = "winformui" },
	iracing = { id = "iracing" },
	state = { id = "state" },
	audio = { id = "audio" },

	-- Twitch. username/token are mandatory. token can be generated here: https://twitchapps.com/tmi/. channel defaults to username
	twitch = { id = "twitch", username = "..", token = "oauth:pvj....", channel = ".." }
}

return cfg;
