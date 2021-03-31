using Slipstream.Components.IRacing.Plugins.GameState;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Threading;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins
{
    internal class IRacingPlugin : BasePlugin
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        private readonly GameState.Mapper Mapper;

        internal const int MAX_CARS = 64;

        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly Trackers.Trackers DataTrackers;
        private readonly bool PublishRawState = false;

        static IRacingPlugin()
        {
            ConfigurationValidator = new DictionaryValidator().PermitBool("send_raw_state");
        }

        public IRacingPlugin(IEventHandlerController eventHandlerController, string id, IIRacingEventFactory eventFactory, IEventBus eventBus, Parameters configuration) : base(eventHandlerController, id, "IRacingPlugin", id)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            Mapper = new GameState.Mapper(new StateFactory());

            ConfigurationValidator.Validate(configuration);

            PublishRawState = configuration.ExtractOrDefault("send_raw_state", false);

            DataTrackers = new Trackers.Trackers(eventBus, eventFactory);

            var IRacingEventHandler = EventHandlerController.Get<EventHandler.IRacing>();

            IRacingEventHandler.OnIRacingCommandSendCarInfo += (s, e) => DataTrackers.SendCarInfo = true;
            IRacingEventHandler.OnIRacingCommandSendTrackInfo += (s, e) => DataTrackers.SendTrackInfo = true;
            IRacingEventHandler.OnIRacingCommandSendWeatherInfo += (s, e) => DataTrackers.SendWeatherInfo = true;
            IRacingEventHandler.OnIRacingCommandSendSessionState += (s, e) => DataTrackers.SendSessionState = true;
            IRacingEventHandler.OnIRacingCommandSendRaceFlags += (s, e) => DataTrackers.SendRaceFlags = true;
        }

        public override void Run()
        {
            var state = Mapper.GetState();

            if (state != null)
            {
                if (PublishRawState)
                {
                    EventBus.PublishEvent(EventFactory.CreateIRacingRaw(state));
                }

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