﻿#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandSay : IEvent
    {
        public string EventType => "AudioCommandSay";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = "INVAILD-INSTANCE-ID";
        public string Message { get; set; } = string.Empty;
        public float Volume { get; set; } = 1.0f;

        public override bool Equals(object? obj)
        {
            return obj is AudioCommandSay say &&
                   EventType == say.EventType &&
                   Message == say.Message &&
                   Volume == say.Volume;
        }

        public override int GetHashCode()
        {
            int hashCode = 1098881401;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Message);
            hashCode = (hashCode * -1521134295) + Volume.GetHashCode();
            return hashCode;
        }
    }
}