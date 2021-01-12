using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class TransmitterPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpClient? Client = null;
        private readonly ITxrxService TxrxService;

        public TransmitterPlugin(string id, ILogger logger, IEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, ITxrxConfiguration txrxConfiguration) : base(id, "TransmitterPlugin", "TransmitterPlugin", "TransmitterPlugin", true)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;
            TxrxService = txrxService;

            var input = txrxConfiguration.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    Logger.Error("TransmitterPlugin: Invalid port in TxrxIpPort provided: {TxrxIpPort}", txrxConfiguration.TxrxIpPort);
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Logger.Error("TransmitterPlugin: Invalid TxrxIpPort provided: {TxrxIpPort}", txrxConfiguration.TxrxIpPort);
            }

            EventHandler.OnDefault += (_, e) => OnEvent(e.Event);

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