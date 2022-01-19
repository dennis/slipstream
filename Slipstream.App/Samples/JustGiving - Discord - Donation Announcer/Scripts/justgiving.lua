local cfg = require("lib/config")
local jg = require("api/justgiving"):instance(cfg.justgiving)
local discord = require("api/discord"):instance(cfg.discord)

--jg:send_all_donations()

addEventHandler("JustGivingDonation", function(event)
    local icon = ":currency_exchange:"

    if event.Amount >= 100 then
        icon = ":star_struck:"
    elseif event.Amount >= 50 then
        icon = ":star2:"
    elseif event.Amount >= 20 then
        icon = ":star:"
    end

    if event.DonorLocalCurrencyCode == event.CurrencyCode then
        discord:channel_id(cfg.ch_donations):send_message(icon .. " __" .. event.DonorDisplayName .. "__ : " .. event.DonorLocalAmount .. " " ..  event.DonorLocalCurrencyCode .. " " ..  event.Message)
    else
        discord:channel_id(cfg.ch_donations):send_message(icon .. " __" .. event.DonorDisplayName .. "__ : " .. event.DonorLocalAmount .. " " ..  event.DonorLocalCurrencyCode .. " (~ " .. event.Amount .. " " .. event.CurrencyCode .. ") " ..  event.Message)
    end    
end)


addEventHandler("JustGivingInfo", function(event)
    discord:channel_id(cfg.ch_donations):send_message("**Donation Total**: " .. event.FundraisingGrandTotal .. " / " .. event.FundraisingTarget .. " " .. event.CurrencyCode)
end)
 