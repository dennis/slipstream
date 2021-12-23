local cfg = require("lib/config")

local web = require("api/webserver"):instance({ id = "webserver", port = 8080})
local twitch = require("api/twitch"):instance(cfg.twitch)

web:serve_directory("/chatstate/", "WebWidgets\\Textbox")
web:serve_websocket("/chatstate/ws")

local focused_msg = "Focused time! Chat will not monitored to keep focus."
local relaxed_msg = "No more focusing, let's chat!'"

function handle(event)
	if event.EventType == "TwitchReceivedMessage" then
		if event.Message == "!chatoff" or event.Message == "!chaton" then
			if event.Moderator or event.Vip or event.Broadcaster then
				if event.Message == "!chatoff" then
					web:broadcast_data("/chatstate/ws", { text = focused_msg})
					twitch:send_channel_message(focused_msg)
				else
					web:broadcast_data("/chatstate/ws", { text = ""})
					twitch:send_channel_message(relaxed_msg)
				end
			else
				twitch:send_channel_message(event.From .. " You are not allowed to use !chaton and !chatoff")
			end
		end
	end
end
