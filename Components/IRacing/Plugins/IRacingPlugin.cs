using Slipstream.Shared;
using System.Threading;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins
{
    internal class IRacingPlugin : BasePlugin
    {
        private readonly GameState.Mapper Mapper = new GameState.Mapper();

        internal const int MAX_CARS = 64;

        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly Trackers.Trackers DataTrackers;

        public IRacingPlugin(IEventHandlerController eventHandlerController, string id, IIRacingEventFactory eventFactory, IEventBus eventBus) : base(eventHandlerController, id, "IRacingPlugin", id)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            DataTrackers = new Trackers.Trackers(eventBus, eventFactory);

            var IRacingEventHandler = EventHandlerController.Get<EventHandler.IRacing>();

            IRacingEventHandler.OnIRacingCommandSendCarInfo += (s, e) => DataTrackers.SendCarInfo = true;
            IRacingEventHandler.OnIRacingCommandSendTrackInfo += (s, e) => DataTrackers.SendTrackInfo = true;
            IRacingEventHandler.OnIRacingCommandSendWeatherInfo += (s, e) => DataTrackers.SendWeatherInfo = true;
            IRacingEventHandler.OnIRacingCommandSendSessionState += (s, e) => DataTrackers.SendSessionState = true;
            IRacingEventHandler.OnIRacingCommandSendRaceFlags += (s, e) => DataTrackers.SendRaceFlags = true;
        }

        public override void Loop()
        {
            var state = Mapper.GetState();

            if (state != null)
            {
                DataTrackers.Handle(state);
            }
            else
            {
                if (DataTrackers.Connected)
                {
                    DataTrackers.Connected = false;
                    EventBus.PublishEvent(EventFactory.CreateIRacingDisconnected());
                }
                Thread.Sleep(5000);
            }
        }
    }
}