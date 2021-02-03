using iRacingSDK;
using Slipstream.Shared;
using System;
using System.Diagnostics;
using System.Threading;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins
{
    internal class IRacingPlugin : BasePlugin
    {
        internal const int MAX_CARS = 64;

        private readonly iRacingConnection Connection = new iRacingConnection();
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
            try
            {
                foreach (var data in Connection.GetDataFeed().WithCorrectedDistances().WithCorrectedPercentages())
                {
                    DataTrackers.HandleSample(data);
                    break; // give control back to PluginWorker
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Attempt to read session data before connection to iRacing" || e.Message == "Attempt to read telemetry data before connection to iRacing")
                {
                    if (DataTrackers.Connected)
                    {
                        DataTrackers.Connected = false;
                        EventBus.PublishEvent(EventFactory.CreateIRacingDisconnected());
                    }
                    Thread.Sleep(5000);
                }

                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}