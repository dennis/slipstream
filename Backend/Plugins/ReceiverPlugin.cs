using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Utility;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class ReceiverPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private string Ip = "";
        private Int32 Port = 42424;
        private TcpListener? Listener;
        private readonly TxrxService TxrxService = new TxrxService();
        private Socket? Client;
        private const int READ_BUFFER_SIZE = 1024 * 16;
        readonly byte[] ReadBuffer = new byte[READ_BUFFER_SIZE];

        public ReceiverPlugin(string id, IEventBus eventBus, TxrxSettings settings) : base(id, "ReceiverPlugin", "ReceiverPlugin", "ReceiverPlugin")
        {
            EventBus = eventBus;

            OnSetting(settings);

            EventHandler.OnSettingTxrxSettings += (s, e) => OnSetting(e.Event);
        }

        public override void OnEnable()
        {
            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginUnregister { Id = "TransmitterPlugin" });
        }

        public override void OnDisable()
        {
            Client?.Dispose();
            Client = null;
        }

        private void OnSetting(TxrxSettings e)
        {
            var input = e.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin: Invalid port in TxrxHost provided: '{e.TxrxIpPort}'" });
                }
            }
            else
            {
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin: Invalid TxrxHost provided: '{e.TxrxIpPort}'" });
            }
        }

        private void SetupListener()
        {
            IPAddress localAddr = IPAddress.Parse(Ip);
            Listener = new TcpListener(localAddr, Port);
            Listener.Start();

            EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin listening on {Listener.LocalEndpoint}" });
        }

        private void AcceptClient()
        {
            Debug.Assert(Client == null);
            Debug.Assert(Listener != null);

            Client = Listener!.AcceptSocket();

            EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin got a connection from {Client.RemoteEndPoint}" });
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
                    EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin disconnected {Client.RemoteEndPoint}" });

                    Client.Dispose();
                    Client = null;
                }
            }
            catch (SocketException e)
            {
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"ReceiverPlugin: Cant receieve data: {e.Message}" });
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
