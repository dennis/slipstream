using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Twitch;
using Slipstream.Shared.Events.Utility;
using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class TwitchPlugin : BasePlugin
    {
        private TwitchClient? Client;
        private readonly IEventBus EventBus;

        private string? TwitchUsername;
        private string? TwitchChannel;
        private string? TwitchToken;
        private bool TwitchLog;
        private bool RequestReconnect;
        private bool AnnouncedConnected = false;

        public TwitchPlugin(string id, IEventBus eventBus, TwitchSettings settings) : base(id, "TwitchPlugin", "TwitchPlugin", "TwitchPlugin")
        {
            EventBus = eventBus;

            EventHandler.OnSettingTwitchSettings += (s, e) => OnTwitchSettings(e.Event);
            EventHandler.OnTwitchCommandSendMessage += (s, e) =>
            {
                if (Client != null && Client.JoinedChannels.Count > 0 && Enabled)
                {
                    Client.SendMessage(TwitchChannel, e.Event.Message);
                }
            };

            OnTwitchSettings(settings);
        }

        private void OnTwitchSettings(TwitchSettings settings)
        {
            {
                if (TwitchUsername != settings.TwitchUsername || TwitchToken != settings.TwitchToken || TwitchChannel != settings.TwitchChannel || TwitchLog != settings.TwitchLog)
                {
                    TwitchUsername = settings.TwitchUsername;
                    TwitchChannel = settings.TwitchChannel;
                    TwitchToken = settings.TwitchToken;
                    TwitchLog = settings.TwitchLog;

                    if (String.IsNullOrEmpty(TwitchChannel))
                        TwitchChannel = TwitchUsername;

                    Disconnect();
                    Connect();
                }
            };
        }

        public override void OnDisable()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            AnnounceDisconnected();
            Client?.Disconnect();
            Client = null;
        }

        public override void OnEnable()
        {
            Connect();
        }

        public override void Loop()
        {
            if(RequestReconnect)
            {
                Disconnect();
                Connect();

                RequestReconnect = false;
            }

            System.Threading.Thread.Sleep(500);
        }

        private void AnnounceConnected()
        {
            if (!AnnouncedConnected)
            {
                EventBus.PublishEvent(new TwitchConnected());
                AnnouncedConnected = true;
            }
        }

        private void AnnounceDisconnected()
        {
            if (AnnouncedConnected)
            {
                EventBus.PublishEvent(new TwitchDisconnected());
                AnnouncedConnected = false;
            }
        }

        private void Connect()
        {
            if (!Enabled)
                return;
            
            if (string.IsNullOrEmpty(TwitchUsername) || string.IsNullOrEmpty(TwitchToken))
                return;

            if (Client != null && Client.IsConnected)
                return;

            ConnectionCredentials credentials = new ConnectionCredentials(TwitchUsername, TwitchToken, "ws://irc-ws.chat.twitch.tv:80");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);
            Client.Initialize(credentials, TwitchChannel);

            Client.OnConnected += OnConnected;
            Client.OnChatCommandReceived += OnChatCommandReceived;
            Client.OnDisconnected += OnDisconnect;
            Client.OnError += OnError;
            Client.OnIncorrectLogin += OnIncorrectLogin;
            Client.OnJoinedChannel += OnJoinedChannel;
            if(TwitchLog)
                Client.OnLog += OnLog;

            Client.Connect();
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            AnnounceConnected();
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            EventBus.PublishEvent(new CommandWriteToConsole { Message = $"Twitch log: {e.Data}" });
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            EventBus.PublishEvent(new CommandWriteToConsole { Message = $"Twitch Error: {e.Exception.Message}" });
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginDisable() { Id = this.Id });
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            EventBus.PublishEvent(new CommandWriteToConsole { Message = $"Twitch Error: {e.Exception.Message}" });
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginDisable() { Id = this.Id });
        }

        private void OnDisconnect(object sender, OnDisconnectedEventArgs e)
        {
            AnnounceDisconnected();
            RequestReconnect = true;
        }

        private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var chatMessage = e.Command.ChatMessage;

            if (chatMessage.IsMe)
                return;

            EventBus.PublishEvent(
                new TwitchReceivedCommand
                {
                    From = chatMessage.DisplayName,
                    Message = chatMessage.Message,
                    Broadcaster = chatMessage.IsBroadcaster,
                    Moderator = chatMessage.IsModerator,
                    Subscriber = chatMessage.IsSubscriber,
                    Vip = chatMessage.IsVip
                });
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            EventBus.PublishEvent(new CommandWriteToConsole { Message = $"Twitch connected as {TwitchUsername} to channel {TwitchChannel}" });
        }
    }
}
