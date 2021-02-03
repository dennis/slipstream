using iRacingSDK;
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

        private static readonly Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum> SessionStateMapping = new Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum>() {
            { iRacingSDK.SessionState.Checkered, IRacingSessionStateEnum.Checkered },
            { iRacingSDK.SessionState.CoolDown, IRacingSessionStateEnum.CoolDown },
            { iRacingSDK.SessionState.GetInCar, IRacingSessionStateEnum.GetInCar },
            { iRacingSDK.SessionState.Invalid, IRacingSessionStateEnum.Invalid },
            { iRacingSDK.SessionState.ParadeLaps, IRacingSessionStateEnum.ParadeLaps },
            { iRacingSDK.SessionState.Racing, IRacingSessionStateEnum.Racing },
            { iRacingSDK.SessionState.Warmup, IRacingSessionStateEnum.Warmup },
        };

        private static readonly Dictionary<string, IRacingSessionTypeEnum> IRacingSessionTypes = new Dictionary<string, IRacingSessionTypeEnum>()
        {
            { "Practice", IRacingSessionTypeEnum.Practice },
            { "Open Qualify", IRacingSessionTypeEnum.OpenQualify },
            { "Lone Qualify", IRacingSessionTypeEnum.LoneQualify },
            { "Offline Testing", IRacingSessionTypeEnum.OfflineTesting },
            { "Race", IRacingSessionTypeEnum.Race },
            { "Warmup", IRacingSessionTypeEnum.Warmup },
        };

        private static readonly Dictionary<string, IRacingCategoryEnum> IRacingCategoryTypes = new Dictionary<string, IRacingCategoryEnum>()
        {
            { "Road", IRacingCategoryEnum.Road },
            { "Oval", IRacingCategoryEnum.Oval },
            { "DirtRoad", IRacingCategoryEnum.DirtRoad },
            { "DirtOval", IRacingCategoryEnum.DirtOval },
        };

        public IRacingSessionTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            var sessionData = data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum];
            var sessionType = IRacingSessionTypes[sessionData.SessionType];
            var sessionTime = data.Telemetry.SessionTime;

            var category = IRacingCategoryTypes[data.SessionData.WeekendInfo.Category];
            var sessionState = data.Telemetry.SessionState;
            var sessionStateMapped = SessionStateMapping[data.Telemetry.SessionState];
            var lapsLimited = sessionData.IsLimitedSessionLaps;
            var timeLimited = sessionData.IsLimitedTime;
            var totalSessionTime = sessionData._SessionTime / 10_000;
            var totalSessionLaps = sessionData._SessionLaps;

            if (sessionType != state.LastSessionType || sessionState != state.LastSessionState || state.SendSessionState)
            {
                IIRacingSessionState @event =
                   sessionType switch
                   {
                       IRacingSessionTypeEnum.Practice => EventFactory.CreateIRacingPractice(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.OpenQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category, openQualify: true),
                       IRacingSessionTypeEnum.LoneQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category, openQualify: false),
                       IRacingSessionTypeEnum.OfflineTesting => EventFactory.CreateIRacingTesting(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.Race => EventFactory.CreateIRacingRace(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.Warmup => EventFactory.CreateIRacingWarmup(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
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