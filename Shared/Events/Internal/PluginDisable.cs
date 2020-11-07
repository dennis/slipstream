﻿#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public class PluginDisable : IEvent
    {
        public Guid Id { get; set; }

        public PluginDisable()
        {
            Id = Guid.NewGuid();
        }
    }
}
