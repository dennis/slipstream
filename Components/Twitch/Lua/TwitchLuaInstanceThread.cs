#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace Slipstream.Components.Twitch.Lua
{
    public class TwitchLuaInstanceThread : BaseInstanceThread, ITwitchLuaInstanceThread
    {
        private readonly IEventHandlerController EventHandlerController;
        private readonly string TwitchToken;
        private readonly string TwitchUsername;
        private readonly string TwitchChannel;
        private readonly bool TwitchLog;
        private readonly ITwitchEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IEventBusSubscription Subscription;
        private bool AnnouncedConnected = false;
        private TwitchClient? Client;
        private bool RequestReconnect = true;
        private readonly List<string> PendingMessages = new List<string>();

        // According to https://dev.twitch.tv/docs/irc/guide we are allowed to send 20
        // commands per 30s to avoid being ignored for 30 mins. These values are for regular
        // users. Moderator got a bit more room.
        private const int CommandsReserved = 5;

        private TimeSpan ThrottleDuration = TimeSpan.FromSeconds(30);
        private int MaximumCommandsWithinThottleDuration = 20 - CommandsReserved; // As API  might send more commands (in addition to message) - we decrease it a bit
        private DateTime ThrottleDurationStart = DateTime.UtcNow;
        private int CommandCountWithinThrottleDuration = 0;

        public TwitchLuaInstanceThread(
            string instanceId,
            string twitchToken,
            string twitchUsername,
            string twitchChannel,
            bool twitchLog,
            Serilog.ILogger logger,
            IEventHandlerController eventHandlerController,
            ITwitchEventFactory eventFactory,
            IEventBus eventBus,
            IEventBusSubscription eventBusSubscription) : base(instanceId, logger, eventHandlerController)
        {
            EventHandlerController = eventHandlerController;
            TwitchToken = twitchToken;
            TwitchUsername = twitchUsername;
            TwitchChannel = twitchChannel;
            TwitchLog = twitchLog;
            EventFactory = eventFactory;
            EventBus = eventBus;
            Subscription = eventBusSubscription;
        }

        protected override void Main()
        {
            var twitchEventHandler = EventHandlerController.Get<EventHandler.Twitch>();
            twitchEventHandler.OnTwitchCommandSendMessage += (_, e) => SendMessage(e.Message);
            twitchEventHandler.OnTwitchCommandSendWhisper += (_, e) => SendWhisper(e.To, e.Message);

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }

                if (RequestReconnect)
                {
                    Disconnect();
                    Connect();

                    RequestReconnect = false;
                }
            }
        }

        new public void Dispose()
        {
            Disconnect();
            base.Dispose();
        }

        private void ThrottleSafe(Action action)
        {
            var now = DateTime.UtcNow;
            if ((ThrottleDurationStart + ThrottleDuration) < now)
            {
                ThrottleDurationStart = DateTime.UtcNow;
                CommandCountWithinThrottleDuration = 0;

                action.Invoke();
            }
            else
            {
                CommandCountWithinThrottleDuration++;

                if (CommandCountWithinThrottleDuration > MaximumCommandsWithinThottleDuration)
                {
                    // We need to sleep until next ThrottleDuration
                    var sleepUntil = ThrottleDurationStart + ThrottleDuration;
                    Thread.Sleep(sleepUntil - now);

                    ThrottleDurationStart = DateTime.UtcNow;
                    CommandCountWithinThrottleDuration = 0;
                }

                action.Invoke();
                Thread.Sleep(250); // Just sleep a bit, so we dont use it all right away
            }
        }

        private void SendMessage(string message)
        {
            if (Client?.JoinedChannels.Count > 0)
            {
                ThrottleSafe(() => Client.SendMessage(TwitchChannel, message));
            }
            else
            {
                PendingMessages.Add(message);
            }
        }

        private void SendWhisper(string to, string message)
        {
            ThrottleSafe(() => Client?.SendWhisper(to, message));
        }

        private void AnnounceConnected()
        {
            Debug.Assert(Client != null);

            if (!AnnouncedConnected)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchConnected(InstanceEnvelope));
                AnnouncedConnected = true;
            }

            if (PendingMessages.Count > 0)
            {
                foreach (var message in PendingMessages)
                {
                    SendMessage(message);
                }
                PendingMessages.Clear();
            }
        }

        private void AnnounceDisconnected()
        {
            if (AnnouncedConnected)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchDisconnected(InstanceEnvelope));
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
            Client.OnGiftedSubscription += OnGiftedSubscription;
            Client.OnIncorrectLogin += OnIncorrectLogin;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnNewSubscriber += OnNewSubscriber;
            Client.OnRaidNotification += OnRaidNotification;
            Client.OnReSubscriber += OnReSubscriber;
            Client.OnWhisperReceived += OnWhisperReceived;
            Client.OnUserStateChanged += Client_OnUserStateChanged;

            if (TwitchLog)
                Client.OnLog += OnLog;

            Client.Connect();

            ThrottleDurationStart = DateTime.UtcNow;
            CommandCountWithinThrottleDuration = 5; // just wildly guessing that we'll use up to 5 commands to connect
        }

        private void Client_OnUserStateChanged(object? sender, OnUserStateChangedArgs e)
        {
            if (e.UserState.IsModerator)
            {
                Logger.Information("I got moderator status. Increasing throttle limits");

                MaximumCommandsWithinThottleDuration = 100 - CommandsReserved;
            }
        }

        private void OnRaidNotification(object? sender, OnRaidNotificationArgs e)
        {
            var @event = EventFactory.CreateTwitchRaided(InstanceEnvelope, e.RaidNotification.DisplayName, int.Parse(e.RaidNotification.MsgParamViewerCount));

            EventBus.PublishEvent(@event);
        }

        private void OnReSubscriber(object? sender, OnReSubscriberArgs e)
        {
            var @event = EventFactory.CreateTwitchUserSubscribed(
                envelope: InstanceEnvelope,
                name: e.ReSubscriber.DisplayName,
                message: e.ReSubscriber.ResubMessage,
                subscriptionPlan: e.ReSubscriber.SubscriptionPlan.ToString(),
                cumulativeMonths: long.Parse(e.ReSubscriber.MsgParamCumulativeMonths),
                streakMonths: long.Parse(e.ReSubscriber.MsgParamStreakMonths),
                systemMessage: e.ReSubscriber.SystemMessageParsed);

            EventBus.PublishEvent(@event);
        }

        private void OnNewSubscriber(object? sender, OnNewSubscriberArgs e)
        {
            var @event = EventFactory.CreateTwitchUserSubscribed(
                envelope: InstanceEnvelope,
                name: e.Subscriber.DisplayName,
                message: e.Subscriber.ResubMessage,
                subscriptionPlan: e.Subscriber.SubscriptionPlan.ToString(),
                cumulativeMonths: 1,
                streakMonths: 1,
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

        private void OnConnected(object? sender, OnConnectedArgs e)
        {
            Logger.Information("Twitch connected as {TwitchUsername} to channel {TwitchChannel}", TwitchUsername, TwitchChannel);
        }

        private void OnDisconnect(object? sender, OnDisconnectedEventArgs e)
        {
            AnnounceDisconnected();
            RequestReconnect = !Stopping;
        }

        private void OnError(object? sender, OnErrorEventArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnGiftedSubscription(object? sender, OnGiftedSubscriptionArgs e)
        {
            var gifter = e.GiftedSubscription.DisplayName;
            if (e.GiftedSubscription.IsAnonymous)
            {
                gifter = "Anonymous";
            }

            var @event = EventFactory.CreateTwitchGiftedSubscription(
                envelope: InstanceEnvelope,
                gifter: gifter,
                subscriptionPlan: e.GiftedSubscription.MsgParamSubPlan.ToString(),
                recipient: e.GiftedSubscription.MsgParamRecipientDisplayName,
                systemMessage: e.GiftedSubscription.SystemMsgParsed
            );

            EventBus.PublishEvent(@event);
        }

        private void OnIncorrectLogin(object? sender, OnIncorrectLoginArgs e)
        {
            Logger.Error("Twitch Error: {Message}}", e.Exception.Message);
        }

        private void OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            AnnounceConnected();
        }

        private void OnLog(object? sender, OnLogArgs e)
        {
            Logger.Verbose("Twitch log: {Data}", e.Data);
        }

        private void OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            var chatMessage = e.ChatMessage;

            if (chatMessage.IsMe)
                return;

            EventBus.PublishEvent(
                EventFactory.CreateTwitchReceivedMessage
                (
                    envelope: InstanceEnvelope,
                    from: chatMessage.DisplayName,
                    message: chatMessage.Message,
                    moderator: chatMessage.IsModerator,
                    subscriber: chatMessage.IsSubscriber,
                    vip: chatMessage.IsVip,
                    broadcaster: chatMessage.IsBroadcaster
                ));
        }

        private void OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
        {
            var message = e.WhisperMessage;

            EventBus.PublishEvent(
                EventFactory.CreateTwitchReceivedWhisper(InstanceEnvelope, message.DisplayName, message.Message)
            );
        }
    }
}