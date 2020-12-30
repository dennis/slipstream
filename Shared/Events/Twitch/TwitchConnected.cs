﻿namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchConnected : IEvent
    {
        public string EventType => "TwitchConnected";
        public bool ExcludeFromTxrx => false;
    }
}
