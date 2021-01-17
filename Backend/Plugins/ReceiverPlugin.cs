using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    public class ReceiverPlugin : BasePlugin
    {
        internal static DictionaryValidator ConfigurationValidator { get; }

        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;
        private readonly string Ip = "";
        private readonly Int32 Port = 42424;
        private TcpListener? Listener;
        private readonly ITxrxService TxrxService;
        private Socket? Client;
        private const int READ_BUFFER_SIZE = 1024 * 16;
        private readonly byte[] ReadBuffer = new byte[READ_BUFFER_SIZE];

        static ReceiverPlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("ip")
                .RequireLong("port")
                ;
        }

        public ReceiverPlugin(string id, ILogger logger, IInternalEventFactory eventFactory, IEventBus eventBus, ITxrxService txrxService, Parameters configuration) : base(id, "ReceiverPlugin", id, "ReceiverPlugin", true)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            TxrxService = txrxService;

            ConfigurationValidator.Validate(configuration);

            Ip = configuration.Extract<string>("ip");
            Port = (int)configuration.Extract<long>("port");

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
            catch (TxrxService.TxrxServiceException e)
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