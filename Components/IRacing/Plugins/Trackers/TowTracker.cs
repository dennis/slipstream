﻿using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class TowTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private bool BeingTowed = false;

        public TowTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            var towTime = currentState.PlayerCarTowTime;

            if (towTime > 0)
            {
                if (!BeingTowed)
                {
                    Debug.WriteLine($"TOW: {towTime}");
                    BeingTowed = true;

                    EventBus.PublishEvent(EventFactory.CreateIRacingTowed(
                        sessionTime: currentState.SessionTime,
                        remainingTowTime: towTime
                    ));
                }
            }
            else
            {
                BeingTowed = false;
            }
        }
    }
}