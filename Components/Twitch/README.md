# Twitch

Very basic twitch integration. Provided by TwitchPlugin. It will send a
`TwitchConnected` when connected and `TwitchDisconnected` when connection is
lost. Messages prefixed with '!' is considered as a command, and will result
`TwitchReceivedCommand` event.

It handles throttling, so if too many messages are sent, it will slown them
down.  If the twitch user configured is an operator, you will get a bit less
restrictive throttling by twitch.

## Lua

<details><summary>twitch:send_channel_message(message)</summary><br />
Sends a public message to the twitch channel configured in settings.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| message   | string |             |

```lua
twitch:send_channel_message("Hello people")
```

This function publishes `TwitchCommandSendMessage` event, that is handled by
TwitchPlugin.

This function is aliased as ``send_twitch_message`` (deprecated)
</details>

<details><summary>twitch:send_whisper_message(to, message)</summary><br />
Sends a public message to a  twitch user.

| Parameter | Type   | Description |
|:----------|:------:|:------------|
| to        | string | Recipient   |
| message   | string |             |

```lua
twitch:send_whisper_message("tntion", "Hello people")
```

This function publishes `TwitchCommandSendWhisper` event, that is handled by
TwitchPlugin.

This function is aliased as ``send_twitch_whisper`` (deprecated)
</details>

## Events

<details><summary>TwitchCommandSendMessage</summary><br />

Sends a message to the connected twitch channel.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant)                             |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Message         | string  | Message                                                           |

**JSON Example:**
`{"EventType":"TwitchCommandSendMessage","Uptime":1742,"Message":"Hello"}`
</details>

<details><summary>TwitchCommandSendWhisper</summary><br />

Sends a whisper to a user.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchCommandSendWhisper` (constant)                             |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| To              | string  | Message                                                           |
| Message         | string  | Message                                                           |

**JSON Example:**
`{"EventType":"TwitchCommandSendWhisper","Uptime":1742,"To":"tntion", "Message":"Hello"}`
</details>

<details><summary>TwitchConnected</summary><br />
Published when we're connected to Twitch.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchConnected` (constant)                                      |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"TwitchConnected"}`
</details>

<details><summary>TwitchDisconnected</summary><br />
We were disconnected from Twitch

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchDisconnected` (constant)                                   |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |

**JSON Example:**
`{"EventType":"TwitchConnected","Uptime":1742}`
</details>

<details><summary>TwitchReceivedMessage</summary><br />

A message received in the channel

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchReceivedMessage` (constant)                                |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| From            | string  | Sender's username                                                 |
| Message         | boolean | Message typed                                                     |
| Moderator       | boolean | Is sender a (twitch) moderator?                                   |
| Subscriber      | boolean | Is sender a (twitch) subscriber?                                  |
| Vip             | boolean | Is sender a (twitch) VIP?                                         |
| Broadcaster     | boolean | Is sender a (twitch) broadcaster?                                 |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","Uptime":1742,"From":"TNTion","Message":"!hello","Moderator":false,"Subscriber":false,"Vip":false,"Broadcaster":true}`
</details>

<details><summary>TwitchReceivedWhisper</summary><br />

A whisper received.

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchReceivedWhisper` (constant)                                |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| From            | string  | Sender's username                                                 |
| Message         | boolean | Message typed                                                     |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","Uptime":1742,"From":"TNTion","Message":"!hello"}`
</details>

<details><summary>TwitchUserSubscribed</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                                       |
|:-----------------|:-------:|:------------------------------------------------------------------|
| EventType        | string  | `TwitchUserSubscribed` (constant)                                 |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Name             | string  | User (re)subscriping                                              |
| Message          | string  | User own message                                                  |
| SystemMessage    | string  | Twitch's  message                                                 |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3`                       |
| CumulativeMonths | long    | Subscription length in months                                     |
| StreakMonths     | long    | How many months are the currect streak                            |

**JSON Example:**
`{"EventType":"TwitchUserSubscribed","Uptime":5114474,"Name":"tntion","Message":"","SystemMessage":"tntion subscribed with Prime. They've subscribed for 7 months, currently on a 7 month streak!","SubscriptionPlan":"Prime","CumulativeMonths":7,"StreakMonths":7}`
</details>

<details><summary>TwitchGiftedSubscription</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                                       |
|:-----------------|:-------:|:------------------------------------------------------------------|
| EventType        | string  | `TwitchGiftedSubscription` (constant)                             |
| Uptime           | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Gifter           | string  | User gifting sub (might be Anonymous)                             |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3`                       |
| SystemMessage    | string  | Twitch's  message                                                 |
| Recipient        | string  | Gift recipient                                                    |

**JSON Example:**
`{"EventType":"TwitchGiftedSubscription","Uptime":3559925,"Gifter":"skidsmarksbot","SubscriptionPlan":"Tier1","Recipient":"luckyguy","SystemMessage":"skidsmarksbot gifted a Tier 1 sub to luckyguy! They have given 7 Gift Subs in the channel!"}`
</details>

<details><summary>TwitchRaided</summary><br />

A user subscribed or resubscribed to the stream

| Name            | Type    | Description                                                       |
|:----------------|:-------:|:------------------------------------------------------------------|
| EventType       | string  | `TwitchRaided` (constant)                                         |
| Uptime          | integer | Time of when the message was sent via Eventbus (in milliseconds). |
| Name            | string  | Who is raiding                                                    |
| ViewerCount     | int     | How many viewers does the raid bring                              |

**JSON Example:**
`{"EventType":"TwitchRaided","Uptime":1986776,"Name":"Sampsoid","ViewerCount":57}`
</details>
