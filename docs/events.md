# Events

Internally, Slipstream is event based. There are two different types of events. 
One describes something that happen. The other describes a request for changing 
something. These are non-command (describes what happened) and command-events
 (request changes).

Each event contains at least three properties. 
 - **EventType**: Name of the event (Such as AudioCommandPlay)
 - **ExcludeFromTxrx**: Is event transmitted via TransmitterPlugin
 - **Uptime**: Time of when the message was sent via Eventbus (in milliseconds). 
   Time is since start of EventBus

Both properties are constants.

In Lua you can receive these events, by implementing a function called `handle` 
taking one argument, which is the event:

```lua
function handle(event)
  if event.EventType == "TwitchReceivedMessage" then
    if(event.Message == "!car") then
      twitch:send_channel_message("Look at the screen")
    end
  end
end
```

The above example will act on `TwitchReceivedMessage`, where if event contains 
the message `!car` it will respond with some static text.


## LuaManager

This coordinates the LuaPlugins in terms of launch new and taking down old ones. It consumes
data from FileMonitorPlugin and Engine primarily.

<details><summary>LuaCommandDeduplicateEvents</summary><br />
Send a set of Events, allowing the plugin to collect them and deduplicate them before
sending then. This is used at startup, to avoid requesting the same data in multiple 
scripts.

| Name            | Type    | Description                                            |
|:----------------|:-------:|:-------------------------------------------------------|
| EventType       | string  | `LuaCommandDeduplicateEvents` (constant)               |
| ExcludeFromTxrx | boolean | true (constant)                                        |
| Events          | string  | JSON serialized version of the events. Separated by \n |

**JSON Example:**  
`{"EventType": "LuaCommandDeduplicateEvents",  "ExcludeFromTxrx": true, "Uptime":1742, "Events": "JSON-ENCODED EVENTS"}`
</details>

## Internal

Do not depend on these. They are internal-

These are generated and consumed by Engine.


<details><summary>InternalCommandPluginRegister</summary><br />
Request a plugin to be consumed

| Name            | Type    | Description                                |
|:----------------|:-------:|:-------------------------------------------|
| EventType       | string  | `InternalCommandPluginRegister` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                            |
| Id              | string  | Id of the Plugin                           |
| PluginName      | string  | Name of the Plugin                         |
| Configuration   | string  | JSON-encoded configuration                 |

**JSON Example:**  
`{ "EventType": "InternalCommandPluginRegister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin"}`
</details>

<details><summary>InternalCommandPluginStates</summary><br />
Request Engine to send state of all plugins

| Name            | Type    | Description                              |
|:----------------|:-------:|:-----------------------------------------|
| EventType       | string  | `InternalCommandPluginStates` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                          |

**JSON Example:**
`{"EventType": "InternalCommandPluginStates", "ExcludeFromTxrx": true}`
</details>

<details><summary>InternalCommandPluginUnregister</summary><br />
Request a plugin to be removed

| Name            | Type    | Description                                  |
|:----------------|:-------:|:---------------------------------------------|
| EventType       | string  | `InternalCommandPluginUnregister` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                              |
| Id              | string  | Id of the Plugin                             |

**JSON Example:**
`{"EventType": "InternalCommandPluginUnregister", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin" }`
</details>

<details><summary>InternalPluginState</summary><br />
Show the state of a plugin. These are published when plugins are 
registered or unregistered or upon request.

| Name            | Type    | Description                                          |
|:----------------|:-------:|:-----------------------------------------------------|
| EventType       | string  | `InternalPluginState` (constant)                     |
| ExcludeFromTxrx | boolean | true (constant)                                      |
| Id              | string  | Id of plugin                                         |
| PluginName      | string  | Name of plugin                                       |
| DisplayName     | string  | Friendly name of the plugin (defaults to PluginName) |
| PluginStatus    | string  | `Registered` or `Unregistered`                       |

**JSON Example:**
`{"EventType": "InternalPluginState", "ExcludeFromTxrx": true, "Uptime":1742, "Id": "AudioPlugin", "PluginName": "AudioPlugin", "DisplayName": "AudioPlugin", "PluginStatus": "Registered"}`
</details>

## Playback

Enables saving and replaying of events. Slipstream stores up-to 10.000 
events that you may save into a plain text file (that you can, and 
should edit). After doing so, you can load these into slipstream again.

<details><summary>PlaybackCommandInjectEvents</summary><br />

Requests that a filename is read and sent as events.


| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Filename         | string  | Filename to read events from|

**JSON Example:**
`{"EventType":"PlaybackCommandInjectEvents","ExcludeFromTxrx":true,"Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T17.53.52.mjson","Uptime":4818}`
</details>

<details><summary>PlaybackCommandSaveEvents</summary><br />

Requests that events already seen, is stored in a file.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Filename         | string  | Filename write events to|

**JSON Example:**
`{"EventType":"PlaybackSaveEvents","ExcludeFromTxrx":true,"Uptime":73571,"Filename":"C:\\Users\\dennis\\Documents\\2021-01-14T21.28.15.mjson"}`
</details>

## Twitch

<details><summary>TwitchCommandSendMessage</summary><br />

Sends a message to the connected twitch channel.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| Message         | string  | Message                               |

**JSON Example:**
`{"EventType":"TwitchCommandSendMessage","ExcludeFromTxrx":false, "Uptime":1742,"Message":"Hello"}`
</details>

<details><summary>TwitchCommandSendWhisper</summary><br />

Sends a whisper to a user.

| Name            | Type    | Description                           |
|:----------------|:-------:|:--------------------------------------|
| EventType       | string  | `TwitchCommandSendWhisper` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                      |
| To              | string  | Message                               |
| Message         | string  | Message                               |

**JSON Example:**
`{"EventType":"TwitchCommandSendWhisper","ExcludeFromTxrx":false, "Uptime":1742,"To":"tntion", "Message":"Hello"}`
</details>

<details><summary>TwitchConnected</summary><br />
Published when we're connected to Twitch.

| Name            | Type    | Description                  |
|:----------------|:-------:|:-----------------------------|
| EventType       | string  | `TwitchConnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)             |

**JSON Example:**
`{"EventType":"TwitchConnected","ExcludeFromTxrx":false}`
</details>

<details><summary>TwitchDisconnected</summary><br />
We were disconnected from Twitch

| Name            | Type    | Description                     |
|:----------------|:-------:|:--------------------------------|
| EventType       | string  | `TwitchDisconnected` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                |

**JSON Example:**
`{"EventType":"TwitchConnected","ExcludeFromTxrx":false, "Uptime":1742}`
</details>

<details><summary>TwitchReceivedMessage</summary><br />

A message received in the channel

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `TwitchReceivedMessage` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| From            | string  | Sender's username                  |
| Message         | boolean | Message typed, including `!`       |
| Moderator       | boolean | Is sender a (twitch) moderator?    |
| Subscriber      | boolean | Is sender a (twitch) subscriber?   |
| Vip             | boolean | Is sender a (twitch) VIP?          |
| Broadcaster     | boolean | Is sender a (twitch) broadcaster?  |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","ExcludeFromTxrx":false, "Uptime":1742,"From":"TNTion","Message":"!hello","Moderator":false,"Subscriber":false,"Vip":false,"Broadcaster":true}`
</details>

<details><summary>TwitchReceivedWhisper</summary><br />

A whisper received.

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `TwitchReceivedWhisper` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| From            | string  | Sender's username                  |

**JSON Example:**
`{"EventType":"TwitchReceivedMessage","ExcludeFromTxrx":false, "Uptime":1742,"From":"TNTion","Message":"!hello"}`
</details>

<details><summary>TwitchUserSubscribed</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                 |
|:-----------------|:-------:|:--------------------------------------------|
| EventType        | string  | `TwitchUserSubscribed` (constant)           |
| ExcludeFromTxrx  | boolean | false (constant)                            |
| Name             | string  | User (re)subscriping                        |
| Message          | string  | User own message                            |
| SystemMessage    | string  | Twitch's  message                           |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3` |
| Months           | long    | Subscription length in months               |
</details>

<details><summary>TwitchGiftedSubscription</summary><br />

A user subscribed or resubscribed to the stream

| Name             | Type    | Description                                 |
|:-----------------|:-------:|:--------------------------------------------|
| EventType        | string  | `TwitchGiftedSubscription` (constant)       |
| ExcludeFromTxrx  | boolean | false (constant)                            |
| Gifter           | string  | User gifting sub (might be Anonymous)       |
| SubscriptionPlan | string  | One of `Prime`, `Tier1`, `Tier2` or `Tier3` |
| SystemMessage    | string  | Twitch's  message                           |
| Recipient        | string  | Gift recipient                              |
</details>

<details><summary>TwitchRaided</summary><br />

A user subscribed or resubscribed to the stream

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `TwitchRaided` (constant)            |
| ExcludeFromTxrx | boolean | false (constant)                     |
| Name            | string  | Who is raiding                       |
| ViewerCount     | int     | How many viewers does the raid bring |
</details>

## UI

<details><summary>UIButtonTriggered</summary><br />
Is sent every time a button is pressed

| Name            | Type    | Description                    |
|:----------------|:-------:|:-------------------------------|
| EventType       | string  | `UIButtonTriggered` (constant) |
| ExcludeFromTxrx | boolean | false (constant)               |
| Text            | string  | Text of the button             |

**JSON Example:**
`{"EventType":"UIButtonTriggered","ExcludeFromTxrx":false, "Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandCreateButton</summary><br />
Create a new button, unless it exists

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `UICommandCreateButton` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| Text            | string  | Text of the button                 |

**JSON Example:**
`{"EventType":"UICommandCreateButton","ExcludeFromTxrx":true, "Uptime":1742,"Text":"Hello"}`
</details>

<details><summary>UICommandDeleteButton</summary><br />
Removes a button again, if it exists

| Name            | Type    | Description                        |
|:----------------|:-------:|:-----------------------------------|
| EventType       | string  | `UICommandDeleteButton` (constant) |
| ExcludeFromTxrx | boolean | false (constant)                   |
| Text            | string  | Text of the button                 |

**JSON Example:**
`{"EventType":"UICommandDeleteButton","ExcludeFromTxrx":true, "Uptime":1742,"Text":"World"}`
</details>

<details><summary>UICommandWriteToConsole</summary><br />
Output something to the console.

| Name            | Type    | Description                          |
|:----------------|:-------:|:-------------------------------------|
| EventType       | string  | `UICommandWriteToConsole` (constant) |
| ExcludeFromTxrx | boolean | true (constant)                      |
| Message         | string  | Message                              |

**JSON Example:**
`{"EventType":"UICommandWriteToConsole","ExcludeFromTxrx":true, "Uptime":1742,"Message":"Hello World"}`
</details>
