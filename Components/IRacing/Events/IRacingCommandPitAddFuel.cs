﻿using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitAddFuel : IEvent
    {
        public string EventType => "IRacingCommandPitFuel";
        public ulong Uptime { get; set; }
        public int AddLiters { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandPitAddFuel info &&
                   EventType == info.EventType &&
                   AddLiters == info.AddLiters;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            return hashCode;
        }
    }
}