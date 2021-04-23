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

        public IRacingInstanceThread(string instanceId, bool publishRawState, ILogger logger, IEventBus eventBus, IIRacingEventFactory eventFactory, IEventHandlerController eventHandlerController) : base(instanceId, logger)
        {
            PublishRawState = publishRawState;
            EventBus = eventBus;
            IRacingEventFactory = eventFactory;
            EventHandlerController = eventHandlerController;
        }

        protected override void Main()
        {
            var dataTrackers = new Trackers.Trackers(EventBus, IRacingEventFactory);

            var iracingEventHandler = EventHandlerController.Get<EventHandler.IRacing>();
            iracingEventHandler.OnIRacingCommandSendCarInfo += (s, e) => dataTrackers.SendCarInfo = true;
            iracingEventHandler.OnIRacingCommandSendTrackInfo += (s, e) => dataTrackers.SendTrackInfo = true;
            iracingEventHandler.OnIRacingCommandSendWeatherInfo += (s, e) => dataTrackers.SendWeatherInfo = true;
            iracingEventHandler.OnIRacingCommandSendSessionState += (s, e) => dataTrackers.SendSessionState = true;
            iracingEventHandler.OnIRacingCommandSendRaceFlags += (s, e) => dataTrackers.SendRaceFlags = true;

            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalShutdown += (_, _e) => Stopping = true;

            var mapper = new Mapper(new StateFactory());

            while (!Stopping)
            {
                var state = mapper.GetState();

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
            }
        }
    }
}