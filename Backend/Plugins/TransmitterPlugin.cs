using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Utility;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class TransmitterPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private string Ip = "";
        private Int32 Port = 42424;
        private TcpClient? Client = null;
        private readonly TxrxService TxrxService = new TxrxService();

        public TransmitterPlugin(string id, IEventBus eventBus, TxrxSettings settings) : base(id, "TransmitterPlugin", "TransmitterPlugin", "TransmitterPlugin")
        {
            EventBus = eventBus;

            OnSetting(settings);

            EventHandler.OnSettingTxrxSettings += (s, e) => OnSetting(e.Event);
            EventHandler.OnDefault += (s, e) => OnEvent(e.Event);

            // Dont send the following events as these will change the plugin state of
            // the receiver
            EventHandler.OnInternalCommandPluginDisable += (s, e) => { };
            EventHandler.OnInternalCommandPluginEnable += (s, e) => { };
            EventHandler.OnInternalCommandPluginRegister += (s, e) => { };
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => { };
            EventHandler.OnInternalCommandPluginStates += (s, e) => { };
            EventHandler.OnInternalFileMonitorFileChanged += (s, e) => { };
            EventHandler.OnInternalFileMonitorFileCreated += (s, e) => { };
            EventHandler.OnInternalFileMonitorFileRenamed += (s, e) => { };
            EventHandler.OnInternalFileMonitorFileDeleted += (s, e) => { };
            EventHandler.OnInternalPluginState += (s, e) => { };

            // Also ignore these as it doesnt make much sense
            EventHandler.OnUtilityCommandWriteToConsole += (s, e) => { };
            EventHandler.OnUtilityCommandSay += (s, e) => { };
            EventHandler.OnUtilityCommandPlayAudio += (s, e) => { };
        }

        private void OnEvent(IEvent @event)
        {
            if (!Enabled || Client == null || !Client.Connected)
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
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Cant send {@event.EventType}: {e.Message}" });
                Reset();
            }
            catch (System.IO.IOException e)
            {
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Cant send {@event.EventType}: {e.Message}" });
                Reset();
            }
        }

        private void OnSetting(TxrxSettings e)
        {
            var input = e.TxrxIpPort.Split(':');

            if (input.Length == 2)
            {
                Ip = input[0];
                if (!Int32.TryParse(input[1], out Port))
                {
                    EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Invalid port in TxrxIpPort provided: '{e.TxrxIpPort}'" });
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Invalid TxrxIpPort provided: '{e.TxrxIpPort}'" });
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
                    EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Connected to '{Ip}:{Port}'" });
                    return;
                }
                else
                {
                    EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Can't connect to '{Ip}:{Port}'" });
                    Client?.Dispose();
                    Client = null;
                }
            }
            catch (Exception e)
            {
                EventBus.PublishEvent(new CommandWriteToConsole() { Message = $"TransmitterPlugin: Error connecting to '{Ip}:{Port}': {e.Message}" });
            }

            Client = null;

            Thread.Sleep(1000);
        }

        public override void OnEnable()
        {
            // To avoid that we get an endless loop, we will Unregister the "other" end in this instance
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginUnregister { Id = "ReceiverPlugin" });
        }

        public override void OnDisable()
        {
            Reset();
        }

        public override void Loop()
        {
            if (!Enabled || Ip.Length == 0)
                return;

            if (Client == null)
            {
                Connect();
            }
        }
    }
}
