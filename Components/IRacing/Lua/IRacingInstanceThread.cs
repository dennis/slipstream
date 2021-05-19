using Serilog;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System.Threading;

namespace Slipstream.Components.IRacing.Lua
{
    public class IRacingInstanceThread : BaseInstanceThread, IIRacingInstanceThread
    {
        private readonly bool PublishRawState;
        private readonly IEventBus EventBus;
        private readonly IIRacingEventFactory IRacingEventFactory;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventBusSubscription Subscription;

        public IRacingInstanceThread(
            string instanceId,
            bool publishRawState,
            ILogger logger,
            IEventBus eventBus,
            IIRacingEventFactory eventFactory,
            IEventHandlerController eventHandlerController,
            IEventBusSubscription eventBusSubscription) : base(instanceId, logger)
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

            var iracingEventHandler = EventHandlerController.Get<EventHandler.IRacing>();
            iracingEventHandler.OnIRacingCommandSendCarInfo += (s, e) => dataTrackers.SendCarInfo = true;
            iracingEventHandler.OnIRacingCommandSendTrackInfo += (s, e) => dataTrackers.SendTrackInfo = true;
            iracingEventHandler.OnIRacingCommandSendWeatherInfo += (s, e) => dataTrackers.SendWeatherInfo = true;
            iracingEventHandler.OnIRacingCommandSendSessionState += (s, e) => dataTrackers.SendSessionState = true;
            iracingEventHandler.OnIRacingCommandSendRaceFlags += (s, e) => dataTrackers.SendRaceFlags = true;
            iracingEventHandler.OnIRacingCommandPitClearAll += (s, e) => iracing.PitClearAll();
            iracingEventHandler.OnIRacingCommandPitClearTyresChange += (s, e) => iracing.PitClearTyreChange();
            iracingEventHandler.OnIRacingCommandPitRequestFastRepair += (s, e) => iracing.PitRequestFastRepair();
            iracingEventHandler.OnIRacingCommandPitAddFuel += (s, e) => iracing.PitAddFuel(e.AddLiters);
            iracingEventHandler.OnIRacingCommandPitChangeLeftFrontTyre += (s, e) => iracing.PitChangeLeftFrontTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeLeftRearTyre += (s, e) => iracing.PitChangeLeftRearTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeRightFrontTyre += (s, e) => iracing.PitChangeRightFrontTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitChangeRightRearTyre += (s, e) => iracing.PitChangeRightRearTyre(e.Kpa);
            iracingEventHandler.OnIRacingCommandPitCleanWindshield += (s, e) => iracing.PitCleanWindshield();

            while (!Stopping)
            {
                var state = iracing.GetState();

                if (state != null)
                {
                    if (PublishRawState)
                    {
                        EventBus.PublishEvent(IRacingEventFactory.CreateIRacingRaw(state));
                    }

                    dataTrackers.Handle(state);
                }
                else
                {
                    if (dataTrackers.Connected)
                    {
                        dataTrackers.Connected = false;
                        EventBus.PublishEvent(IRacingEventFactory.CreateIRacingDisconnected());
                    }
                    Thread.Sleep(5000);
                }

                IEvent? @event = Subscription.NextEvent(5);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }
        }
    }
}