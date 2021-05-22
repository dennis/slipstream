﻿using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitChangeLeftRearTyre : IEvent
    {
        public string EventType => "IRacingCommandPitLeftRearTyre";
        public ulong Uptime { get; set; }
        public int Kpa { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandPitChangeLeftRearTyre info &&
                   EventType == info.EventType &&
                   Kpa == info.Kpa;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            return hashCode;
        }
    }
}