using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;
using System;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
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

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            var sessionType = currentState.SessionType;
            var sessionState = currentState.SessionState;

            if (sessionType != state.LastSessionType || sessionState != state.LastSessionState)
            {
                IIRacingSessionState @event = GenerateEvent(currentState, envelope);

                EventBus.PublishEvent(@event);

                // If we change from practice to qualify, we need to reset car info
                if (sessionType != state.LastSessionType)
                {
                    foreach (var carState in state.CarsTracked)
                    {
                        carState.Value.ClearState();
                    }
                }

                for (int i = 0; i < Constants.MAX_CARS; i++)
                {
                    state.LastPositionInClass[i] = 0;
                    state.LastPositionInRace[i] = 0;
                }

                state.LastSessionType = sessionType;
                state.LastSessionState = sessionState;
            }
        }

        private IIRacingSessionState GenerateEvent(GameState.IState currentState, IEventEnvelope envelope)
        {
            var sessionType = currentState.SessionType;
            var sessionState = currentState.SessionState;
            var sessionData = currentState.CurrentSession;
            var sessionTime = currentState.SessionTime;

            var category = currentState.RaceCategory;
            var lapsLimited = sessionData.LapsLimited;
            var timeLimited = sessionData.TimeLimited;
            var totalSessionTime = sessionData.TotalSessionTime;
            var totalSessionLaps = sessionData.TotalSessionLaps;

            IIRacingSessionState @event =
               sessionType switch
               {
                   IRacingSessionTypeEnum.Practice => EventFactory.CreateIRacingPractice(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                   IRacingSessionTypeEnum.OpenQualify => EventFactory.CreateIRacingQualify(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category, openQualify: true),
                   IRacingSessionTypeEnum.LoneQualify => EventFactory.CreateIRacingQualify(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category, openQualify: false),
                   IRacingSessionTypeEnum.OfflineTesting => EventFactory.CreateIRacingTesting(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                   IRacingSessionTypeEnum.Race => EventFactory.CreateIRacingRace(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                   IRacingSessionTypeEnum.Warmup => EventFactory.CreateIRacingWarmup(envelope: envelope, sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionState, category: category),
                   _ => throw new NotImplementedException(),
               };
            return @event;
        }

        public void Request(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
            if (request != IIRacingDataTracker.RequestType.SessionState)
                return;

            IIRacingSessionState @event = GenerateEvent(currentState, envelope);

            EventBus.PublishEvent(@event);
        }
    }
}