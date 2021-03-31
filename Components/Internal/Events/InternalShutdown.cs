﻿#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Internal.Events
{
    public class InternalShutdown : IEvent
    {
        public string EventType => "InternalShutdown";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InternalShutdown shutdown &&
                   EventType == shutdown.EventType &&
                   ExcludeFromTxrx == shutdown.ExcludeFromTxrx;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            return hashCode;
        }
    }
}