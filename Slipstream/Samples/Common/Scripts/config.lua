local cfg = {
	-- This does not need to be changed
	util = { id = "util" },
	winformui = { id = "winformui" },
	iracing = { id = "iracing" },
	state = { id = "state" },
	audio = { id = "audio" },

	-- Just Giving. You need an account to get an appid
	justgiving = { id = "justgiving", appid = "APPID", page = "raceformentalhealth2021" },

	-- Twitch. username/token are mandatory. token can be generated here: https://twitchapps.com/tmi/. channel defaults to username
	twitch = { id = "twitch", username = "..", token = "oauth:pvj....", channel = ".." },

	-- Discord
	discord = { id = "discord", token = "TOKEN"},
	ch_donations = 99393393939 -- discord channel id
}

return cfg;
