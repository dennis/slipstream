using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Events.Twitch;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
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
        private readonly IEventBus EventBus;
        private readonly ITwitchEventFactory EventFactory;
        private readonly ILogger Logger;
        private readonly string TwitchChannel;
        private readonly bool TwitchLog;
        private readonly string TwitchToken;
        private readonly string TwitchUsername;
        private bool AnnouncedConnected = false;
        private TwitchClient? Client;
        private bool RequestReconnect = true;

        static TwitchPlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("twitch_username")
                .RequireString("twitch_channel")
                .RequireString("twitch_token")
                .PermitBool("twitch_log")
                ;
        }

        public TwitchPlugin(string id, ILogger logger, ITwitchEventFactory eventFactory, IEventBus eventBus, Parameters configuration) : base(id, "TwitchPlugin", id, "TwitchPlugin", true)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;

            ConfigurationValidator.Validate(configuration);

            var twitchEventHandler = EventHandler.Get<Shared.EventHandlers.Twitch>();

            twitchEventHandler.OnTwitchCommandSendMessage += (_, e) =>
            {
                if (Client?.JoinedChannels.Count > 0)
                {
                    Client.SendMessage(TwitchChannel, e.Event.Message);
                }
            };
            twitchEventHandler.OnTwitchCommandSendWhisper += (_, e) => Client?.SendWhisper(e.Event.To, e.Event.Message);

            TwitchUsername = configuration.Extract<string>("twitch_username");
            TwitchChannel = configuration.Extract<string>("twitch_channel");
            TwitchToken = configuration.Extract<string>("twitch_token");
            TwitchLog = configuration.ExtractOrDefault("twitch_log", false);
        }

        public static DictionaryValidator ConfigurationValidator { get; }

        public override void Dispose()
        {
            Disconnect();
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
            Client.OnDisconnected += OnDisconnect;
            Client.OnError += OnError;
            Client.OnIncorrectLogin += OnIncorrectLogin;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnNewSubscriber += OnNewSubscriber;
            Client.OnReSubscriber += OnReSubscriber;
            Client.OnWhisperReceived += OnWhisperReceived;

            if (TwitchLog)
                Client.OnLog += OnLog;

            Client.Connect();
        }

        private void OnReSubscriber(object sender, OnReSubscriberArgs e)
        {
            var @event = EventFactory.CreateTwitchUserSubscribed(
                name: e.ReSubscriber.DisplayName,
                message: e.ReSubscriber.ResubMessage,
                subscriptionPlan: e.ReSubscriber.SubscriptionPlan.ToString(),
                months: e.ReSubscriber.Months,
                systemMessage: e.ReSubscriber.SystemMessageParsed);

            EventBus.PublishEvent(@event);
        }

        private void OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            var @event = EventFactory.CreateTwitchUserSubscribed(
                name: e.Subscriber.DisplayName,
                message: e.Subscriber.ResubMessage,
                subscriptionPlan: e.Subscriber.SubscriptionPlan.ToString(),
                months: 1,
                systemMessage: e.Subscriber.SystemMessageParsed
            );

            EventBus.PublishEvent(@event);
        }

        private void Disconnect()
        {
            AnnounceDisconnected();
            Client?.Disconnect();
            Client = null;
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            Logger.Information("Twitch connected as {TwitchUsername} to channel {TwitchChannel}", TwitchUsername, TwitchChannel);
        }

        private void OnDisconnect(object sender, OnDisconnectedEventArgs e)
        {
            AnnounceDisconnected();
            RequestReconnect = true;
        }

        private void OnError(object sender, OnErrorEventArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            AnnounceConnected();
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            Logger.Verbose("Twitch log: {Data}", e.Data);
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
    }
}