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
    internal class TransmitterPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpClient? Client = null;
        private readonly ITxrxService TxrxService;

        public TransmitterPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, ITxrxConfiguration txrxConfiguration) : base(id, "TransmitterPlugin", "TransmitterPlugin", "TransmitterPlugin", true)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            TxrxService = txrxService;

            var input = txrxConfiguration.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Invalid port in TxrxIpPort provided: '{txrxConfiguration.TxrxIpPort}'"));
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Invalid TxrxIpPort provided: '{txrxConfiguration.TxrxIpPort}'"));
            }

            EventHandler.OnDefault += (s, e) => OnEvent(e.Event);

            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister("ReceiverPlugin"));
        }

        private void OnEvent(IEvent @event)
        {
            if (Client == null || !Client.Connected || @event.ExcludeFromTxrx)
                return;

            try
            {
                string json = TxrxService.Serialize(@event);

                Debug.WriteLine($"Sending '{json}'");

                byte[] data = System.Text.Encoding.Unicode.GetBytes(json);

                Client.GetStream().Write(data, 0, data.Length);
            }
            catch (SocketException e)
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Cant send {@event.EventType}: {e.Message}"));
                Reset();
            }
            catch (System.IO.IOException e)
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Cant send {@event.EventType}: {e.Message}"));
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
                    EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Connected to '{Ip}:{Port}'"));
                    return;
                }
                else
                {
                    EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Can't connect to '{Ip}:{Port}'"));
                    Client?.Dispose();
                    Client = null;
                }
            }
            catch (Exception e)
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"TransmitterPlugin: Error connecting to '{Ip}:{Port}': {e.Message}"));
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