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

Look at the different components to see which events and lua functions are 
available.

# Components

 * [Audio](/Components/Audio/README.md)
 * [FileMonitor](/Components/FileMonitor/README.md)
 * [Internal](/Components/Internal/README.md)
 * [IRacing](/Components/IRacing/README.md)
 * [Lua](/Components/Lua/README.md)
 * [Playback](/Components/Playback/README.md)
 * [Twitch](/Components/Twitch/README.md)
 * [Txrx](/Components/Txrx/README.md)
 * [UI](/Components/UI/README.md)
