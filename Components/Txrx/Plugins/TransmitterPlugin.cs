using Serilog;
using Slipstream.Backend.Plugins;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

#nullable enable

namespace Slipstream.Components.Txrx.Plugins
{
    public class TransmitterPlugin : BasePlugin
    {
        private static DictionaryValidator ConfigurationValidator { get; }

        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpClient? Client = null;
        private readonly ITxrxService TxrxService;

        static TransmitterPlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("ip")
                .RequireLong("port")
                ;
        }

        public TransmitterPlugin(IEventHandlerController eventHandlerController, string id, ILogger logger, IInternalEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, Parameters configuration) : base(eventHandlerController, id, "TransmitterPlugin", id, true)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;
            TxrxService = txrxService;

            ConfigurationValidator.Validate(configuration);

            Ip = configuration.Extract<string>("ip");
            Port = (int)configuration.Extract<long>("port");

            EventHandlerController.OnDefault += (_, e) => OnEvent(e.Event);

            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister("ReceiverPlugin"));
        }

        private void OnEvent(IEvent @event)
        {
            if (Client?.Connected != true || @event.ExcludeFromTxrx)
                return;

            try
            {
                string json = TxrxService.Serialize(@event);

                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

                Client.GetStream().Write(data, 0, data.Length);
            }
            catch (SocketException e)
            {
                Logger.Error("TransmitterPlugin: Cant send {EventType}: {Message}", @event.EventType, e.Message);

                Reset();
            }
            catch (System.IO.IOException e)
            {
                Logger.Error("TransmitterPlugin: Cant send {EventType}: {Message}", @event.EventType, e.Message);

                Reset();
            }
        }

        private void Reset()
        {
            Client?.Close();
            Client?.Dispose();
            Client = null;
        }

        private void Connect()
        {
            Debug.Assert(Client == null);

            try
            {
                Client = new TcpClient
                {
                    SendTimeout = 500,
                    ExclusiveAddressUse = true,
                };

                var result = Client.BeginConnect(Ip, Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
                Client.EndConnect(result);

                if (success)
                {
                    Logger.Information("TransmitterPlugin: Connected to {Ip}:{Port}", Ip, Port);
                    return;
                }
                else
                {
                    Logger.Error("TransmitterPlugin: Can't connect to {Ip}:{Port}", Ip, Port);
                    Client?.Dispose();
                    Client = null;
                }
            }
            catch (Exception e)
            {
                Logger.Error("TransmitterPlugin: Error connecting to {Ip}:{Port}: {Message}", Ip, Port, e.Message);
            }

            Client = null;

            Thread.Sleep(1000);
        }

        public override void Loop()
        {
            if (Ip.Length == 0)
                return;

            if (Client == null)
            {
                Connect();
            }
        }
    }
}