﻿using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.UnitTests.Components.IRacing.Plugins.Trackers
{
    public class TestEventBus : IEventBus
    {
        public bool Enabled { get; set; } = true;
        public List<IEvent> Events { get; } = new List<IEvent>();

        public void PublishEvent(IEvent e)
        {
            if (!Enabled)
                return;

            Events.Add(e);
        }

        public IEventBusSubscription RegisterListener(bool fromBeginning = false)
        {
            throw new System.NotImplementedException();
        }

        public void UnregisterSubscription(IEventBusSubscription eventBusSubscription)
        {
            throw new System.NotImplementedException();
        }
    }
}