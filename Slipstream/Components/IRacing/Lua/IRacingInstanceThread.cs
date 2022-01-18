#nullable enable

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Components.Internal.EventFactory;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System.Threading;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingInstanceThread : BaseInstanceThread, IIRacingInstanceThread
    {
        private readonly bool PublishRawState;
        private readonly IIRacingEventFactory IRacingEventFactory;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventBusSubscription Subscription;

        public IRacingInstanceThread(
            string luaLibraryName,
            string instanceId,
            bool publishRawState,
            ILogger logger,
            IEventBus eventBus,
            IIRacingEventFactory eventFactory,
            IEventHandlerController eventHandlerController,
            IEventBusSubscription eventBusSubscription,
            IInternalEventFactory internalEventFactory
        ) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            PublishRawState = publishRawState;
            EventBus = eventBus;
            IRacingEventFactory = eventFactory;
            EventHandlerController = eventHandlerController;
            Subscription = eventBusSubscription;
        }

        protected override void Main()
        {
            var dataTrackers = new Trackers.Trackers(EventBus, IRacingEventFactory);
            var iracing = new IRacingFacade(new StateFactory());
            var state = iracing.GetState();

            var iracingEventHandler = EventHandlerController.Get<EventHandler.IRacing>();
            iracingEventHandler.OnIRacingCommandSendCarInfo += (_, e) => dataTrackers.Request(state, e.Envelope.Reply(InstanceId), Trackers.IIRacingDataTracker.RequestType.CarInfo);
            iracingEventHandler.OnIRacingCommandSendTrackInfo += (_, e) => dataTrackers.Request(state, e.Envelope.Reply(InstanceId), Trackers.IIRacingDataTracker.RequestType.TrackInfo);
            iracingEventHandler.OnIRacingCommandSendWeatherInfo += (_, e) => dataTrackers.Request(state, e.Envelope.Reply(InstanceId), Trackers.IIRacingDataTracker.RequestType.WeatherInfo);
            iracingEventHandler.OnIRacingCommandSendSessionState += (_, e) => dataTrackers.Request(state, e.Envelope.Reply(InstanceId), Trackers.IIRacingDataTracker.RequestType.SessionState);
            iracingEventHandler.OnIRacingCommandSendRaceFlags += (_, e) => dataTrackers.Request(state, e.Envelope.Reply(InstanceId), Trackers.IIRacingDataTracker.RequestType.RaceFlags);
            iracingEventHandler.OnIRacingCommandPitClearAll += (_, e) => iracing.PitClearAll();
            iracingEventHandler.OnIRacingCommandPitClearTyresChange += (_, e) => iracing.PitClearTyreChange();
            iracingEventHandler.OnIRacingCommandPitRequestFastRepair += (_, e) => iracing.PitRequestFastRepair();
            iracingEventHandler.OnIRacingCommandPitAddFuel += (_, e) => iracing.PitAddFuel(e.AddLiters);
            iracingEventHandler.OnIRacingCommandPitChangeLeftFrontTyre += (_, e) => iracing.PitChangeLeftFrontTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeLeftRearTyre += (_, e) => iracing.PitChangeLeftRearTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeRightFrontTyre += (_, e) => iracing.PitChangeRightFrontTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeRightRearTyre += (_, e) => iracing.PitChangeRightRearTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitCleanWindshield += (_, e) => iracing.PitCleanWindshield();

            while (!Stopping)
            {
                if (state != null)
                {
                    if (PublishRawState)
                    {
                        EventBus.PublishEvent(IRacingEventFactory.CreateIRacingRaw(InstanceEnvelope, state));
                    }

                    dataTrackers.Handle(state, InstanceEnvelope);
                }
                else
                {
                    if (dataTrackers.Connected)
                    {
                        dataTrackers.Connected = false;
                        EventBus.PublishEvent(IRacingEventFactory.CreateIRacingDisconnected(InstanceEnvelope));
                    }
                    Thread.Sleep(5000);
                }

                IEvent? @event = Subscription.NextEvent(5);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }

                state = iracing.GetState();
            }
        }
    }
}