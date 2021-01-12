using Serilog;
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
        private readonly ILogger Logger;
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;

        private readonly string TwitchUsername;
        private readonly string TwitchChannel;
        private readonly string TwitchToken;
        private readonly bool TwitchLog;
        private bool RequestReconnect = true;
        private bool AnnouncedConnected = false;

        public TwitchPlugin(string id, ILogger logger, IEventFactory eventFactory, IEventBus eventBus, ITwitchConfiguration twitchConfiguration) : base(id, "TwitchPlugin", "TwitchPlugin", "TwitchPlugin", true)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;

            EventHandler.OnTwitchCommandSendMessage += (_, e) =>
            {
                if (Client?.JoinedChannels.Count > 0)
                {
                    Client.SendMessage(TwitchChannel, e.Event.Message);
                }
            };
            EventHandler.OnTwitchCommandSendWhisper += (_, e) => Client?.SendWhisper(e.Event.To, e.Event.Message);

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

            if (Client?.IsConnected == true)
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
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnWhisperReceived += OnWhisperReceived;
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
            Logger.Verbose("Twitch log: {Data}", e.Data);
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnDisconnect(object sender, OnDisconnectedEventArgs e)
        {
            AnnounceDisconnected();
            RequestReconnect = true;
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var chatMessage = e.ChatMessage;

            if (chatMessage.IsMe)
                return;

            EventBus.PublishEvent(
                EventFactory.CreateTwitchReceivedMessage
                (
                    from: chatMessage.DisplayName,
                    message: chatMessage.Message,
                    moderator: chatMessage.IsModerator,
                    subscriber: chatMessage.IsSubscriber,
                    vip: chatMessage.IsVip,
                    broadcaster: chatMessage.IsBroadcaster
                ));
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            var message = e.WhisperMessage;

            EventBus.PublishEvent(
                EventFactory.CreateTwitchReceivedWhisper(message.DisplayName, message.Message)
            );
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            Logger.Information("Twitch connected as {TwitchUsername} to channel {TwitchChannel}", TwitchUsername, TwitchChannel);
        }

        public override void Dispose()
        {
            Disconnect();
        }
    }
}