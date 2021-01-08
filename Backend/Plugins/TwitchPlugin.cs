using Slipstream.Shared;
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
    internal class TwitchPlugin : BasePlugin
    {
        private TwitchClient? Client;
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;

        private readonly string TwitchUsername;
        private readonly string TwitchChannel;
        private readonly string TwitchToken;
        private readonly bool TwitchLog;
        private bool RequestReconnect;
        private bool AnnouncedConnected = false;

        public TwitchPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, ITwitchConfiguration twitchConfiguration) : base(id, "TwitchPlugin", "TwitchPlugin", "TwitchPlugin", true)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            EventHandler.OnTwitchCommandSendMessage += (s, e) =>
            {
                if (Client != null && Client.JoinedChannels.Count > 0)
                {
                    Client.SendMessage(TwitchChannel, e.Event.Message);
                }
            };

            TwitchUsername = twitchConfiguration.TwitchUsername;
            TwitchChannel = twitchConfiguration.TwitchChannel;
            TwitchToken = twitchConfiguration.TwitchToken;
            TwitchLog = twitchConfiguration.TwitchLog;
            if (String.IsNullOrEmpty(TwitchChannel))
                TwitchChannel = TwitchUsername;
        }

        private void Disconnect()
        {
            AnnounceDisconnected();
            Client?.Disconnect();
            Client = null;
        }

        public override void Loop()
        {
            if (RequestReconnect)
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
                EventBus.PublishEvent(EventFactory.CreateTwitchConnected());
                AnnouncedConnected = true;
            }
        }

        private void AnnounceDisconnected()
        {
            if (AnnouncedConnected)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchDisconnected());
                AnnouncedConnected = false;
            }
        }

        private void Connect()
        {
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
            if (TwitchLog)
                Client.OnLog += OnLog;

            Client.Connect();
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            AnnounceConnected();
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"Twitch log: {e.Data}"));
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"Twitch Error: {e.Exception.Message}"));
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"Twitch Error: {e.Exception.Message}"));
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
                EventFactory.CreateTwitchReceivedCommand
                (
                    from: chatMessage.DisplayName,
                    message: chatMessage.Message,
                    broadcaster: chatMessage.IsBroadcaster,
                    moderator: chatMessage.IsModerator,
                    subscriber: chatMessage.IsSubscriber,
                    vip: chatMessage.IsVip
                ));
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"Twitch connected as {TwitchUsername} to channel {TwitchChannel}"));
        }
    }
}