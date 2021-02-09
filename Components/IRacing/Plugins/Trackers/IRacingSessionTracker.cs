using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class IRacingSessionTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public IRacingSessionTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            var sessionData = currentState.CurrentSession;
            var sessionType = currentState.SessionType;
            var sessionTime = currentState.SessionTime;

            var category = currentState.RaceCategory;
            var sessionState = currentState.SessionState;
            var lapsLimited = sessionData.LapsLimited;
            var timeLimited = sessionData.TimeLimited;
            var totalSessionTime = sessionData.TotalSessionTime;
            var totalSessionLaps = sessionData.TotalSessionLaps;

            if (sessionType != state.LastSessionType || sessionState != state.LastSessionState || state.SendSessionState)
            {
                IIRacingSessionState @event =
                   sessionType switch
                   {
                       IRacingSessionTypeEnum.Practice => EventFactory.CreateIRacingPractice(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                       IRacingSessionTypeEnum.OpenQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category, openQualify: true),
                       IRacingSessionTypeEnum.LoneQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category, openQualify: false),
                       IRacingSessionTypeEnum.OfflineTesting => EventFactory.CreateIRacingTesting(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                       IRacingSessionTypeEnum.Race => EventFactory.CreateIRacingRace(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                       IRacingSessionTypeEnum.Warmup => EventFactory.CreateIRacingWarmup(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                       _ => throw new NotImplementedException(),
                   };

                EventBus.PublishEvent(@event);

                // If we change from practice to qualify, we need to reset car info
                if (sessionType != state.LastSessionType)
                {
                    foreach (var carState in state.CarsTracked)
                    {
                        carState.Value.ClearState();
                    }
                }

                for (int i = 0; i < IRacingPlugin.MAX_CARS; i++)
                {
                    state.LastPositionInClass[i] = 0;
                    state.LastPositionInRace[i] = 0;
                }

                state.LastSessionType = sessionType;
                state.LastSessionState = sessionState;
                state.SendSessionState = false;
            }
        }
    }
}