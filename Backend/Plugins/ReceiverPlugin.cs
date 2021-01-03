using Slipstream.Backend.Services;
using Slipstream.Shared;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class ReceiverPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpListener? Listener;
        private readonly ITxrxService TxrxService;
        private Socket? Client;
        private const int READ_BUFFER_SIZE = 1024 * 16;
        readonly byte[] ReadBuffer = new byte[READ_BUFFER_SIZE];

        public ReceiverPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, ITxrxConfiguration txrxConfiguration) : base(id, "ReceiverPlugin", "ReceiverPlugin", "ReceiverPlugin")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            TxrxService = txrxService;

            var input = txrxConfiguration.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin: Invalid port in TxrxHost provided: '{txrxConfiguration.TxrxIpPort}'"));
                }
            }
            else
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin: Invalid TxrxHost provided: '{txrxConfiguration.TxrxIpPort}'"));
            }
        }

        public override void OnEnable()
        {
            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister("TransmitterPlugin"));
        }

        public override void OnDisable()
        {
            Client?.Dispose();
            Client = null;
        }

        private void SetupListener()
        {
            IPAddress localAddr = IPAddress.Parse(Ip);
            Listener = new TcpListener(localAddr, Port);
            Listener.Start();

            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin listening on {Listener.LocalEndpoint}"));
        }

        private void AcceptClient()
        {
            Debug.Assert(Client == null);
            Debug.Assert(Listener != null);

            Client = Listener!.AcceptSocket();

            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin got a connection from {Client.RemoteEndPoint}"));
        }

        private void ReadData()
        {
            Debug.Assert(Client != null);

            try
            {
                if (Client!.Available == 0)
                    return;

                int length = Client.Receive(ReadBuffer);

                string json = System.Text.Encoding.Unicode.GetString(ReadBuffer, 0, length);

                TxrxService.Parse(json, (@event) => EventBus.PublishEvent(@event));

                // Check if disconnceted
                if (Client!.Poll(200, SelectMode.SelectRead) && Client!.Available == 0)
                {
                    EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin disconnected {Client.RemoteEndPoint}"));

                    Client.Dispose();
                    Client = null;
                }
            }
            catch (SocketException e)
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"ReceiverPlugin: Cant receieve data: {e.Message}"));
            }
        }

        public override void Loop()
        {
            if (!Enabled || Ip.Length == 0)
                return;

            if (Listener == null)
            {
                SetupListener();
            }

            if (Listener != null && Client == null)
            {
                AcceptClient();
            }

            if (Client != null)
            {
                ReadData();
            }
        }
    }
}
