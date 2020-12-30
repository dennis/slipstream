﻿namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchDisconnected : IEvent
    {
        public string EventType => "TwitchDisconnected";
        public bool ExcludeFromTxrx => false;
    }
}
