using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class ReceiverPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpListener? Listener;
        private readonly ITxrxService TxrxService;
        private Socket? Client;
        private const int READ_BUFFER_SIZE = 1024 * 16;
        private readonly byte[] ReadBuffer = new byte[READ_BUFFER_SIZE];

        public ReceiverPlugin(string id, ILogger logger, IEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, ITxrxConfiguration txrxConfiguration) : base(id, "ReceiverPlugin", "ReceiverPlugin", "ReceiverPlugin", true)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            TxrxService = txrxService;

            var input = txrxConfiguration.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    Logger.Error("ReceiverPlugin: Invalid port in TxrxHost provided: {TxrxIpPort}", txrxConfiguration.TxrxIpPort);
                }
            }
            else
            {
                Logger.Error("ReceiverPlugin: Invalid TxrxHost provided: {TxrxIpPort}", txrxConfiguration.TxrxIpPort);
            }

            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginUnregister("TransmitterPlugin"));
        }

        private void SetupListener()
        {
            IPAddress localAddr = IPAddress.Parse(Ip);
            Listener = new TcpListener(localAddr, Port);
            Listener.Start();

            Logger.Information("ReceiverPlugin listening on {Endpoint}", Listener.LocalEndpoint);
        }

        private void AcceptClient()
        {
            Debug.Assert(Client == null);
            Debug.Assert(Listener != null);

            if (!Listener!.Pending())
                return;

            Client = Listener!.AcceptSocket();

            Logger.Information("ReceiverPlugin got a connection from {Endpoint}", Client.RemoteEndPoint);
        }

        private void ReadData()
        {
            Debug.Assert(Client != null);

            try
            {
                if (Client!.Available == 0)
                {
                    // Check if disconnceted
                    if (Client!.Poll(200, SelectMode.SelectRead) && Client!.Available == 0)
                    {
                        Logger.Information("ReceiverPlugin disconnected {Endpoint}", Client.RemoteEndPoint);

                        Client.Dispose();
                        Client = null;
                    }

                    return;
                }

                int length = Client.Receive(ReadBuffer);

                string json = System.Text.Encoding.UTF8.GetString(ReadBuffer, 0, length);

                TxrxService.Parse(json, (@event) => EventBus.PublishEvent(@event));
            }
            catch (SocketException e)
            {
                Logger.Information("ReceiverPlugin: Cant receieve data: {Message}", e.Message);
            }
            catch(TxrxService.TxrxServiceException e)
            {
                Logger.Information("ReceiverPlugin: Error parsing data {Message}", e.Message);
            }
        }

        public override void Loop()
        {
            if (Ip.Length == 0)
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