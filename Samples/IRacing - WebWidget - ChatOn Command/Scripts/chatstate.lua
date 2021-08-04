local cfg = require("lib/config")

local widget = require("api/webwidget"):instance({ id = "focused-mode", type = "textbox"})
local twitch = require("api/twitch"):instance(cfg.twitch)

local focused_msg = "Focused time! Chat will not monitored to keep focus."
local relaxed_msg = "No more focusing, let's chat!'"

function handle(event)
	if event.EventType == "TwitchReceivedMessage" then
		if event.Message == "!chatoff" or event.Message == "!chaton" then
			if event.Moderator or event.Vip or event.Broadcaster then
				if event.Message == "!chatoff" then
					widget:send({ text = focused_msg})
					twitch:send_channel_message(focused_msg)
				else
					widget:send({ text = ""})
					twitch:send_channel_message(relaxed_msg)
				end
			else
				twitch:send_channel_message(event.From .. " You are not allowed to use !chaton and !chatoff")
			end
		end
	end
end
