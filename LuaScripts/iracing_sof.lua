-- Implements a "!sof" command for twitch users to see the current strength-of-field
local common = require("lib/common")

local cars = {}

function round(num, numDecimalPlaces)
  local mult = 10^(numDecimalPlaces or 0)
  return math.floor(num * mult + 0.5) / mult
end

local sof = common.with_cooldown("!sof", 30, function()
    local base_sof = 1600 / math.log(2)
    local sof_exp_sum = 0
    local count = 0

    for k, ir in pairs(cars) do
      if ir > 0 then
        sof_exp_sum = sof_exp_sum + math.exp(-ir / base_sof)
        count = count + 1
      end
    end

    local sof = round(math.floor(base_sof * math.log(count / sof_exp_sum)))

    twitch:send_channel_message("SOF is " .. string.format("%d", sof))
end)

iracing:send_car_info()

function handle(event)
  if event.EventType == "TwitchReceivedMessage" then
    if string.starts(event.Message, "!sof") then
      sof()
    end
  elseif event.EventType == "IRacingCarInfo" and event.Spectator == false then
	cars[event.CarIdx] = event.CurrentDriverIRating
  elseif event.EventType == "IRacingDisconnected" then
	cars = {}
  end
end