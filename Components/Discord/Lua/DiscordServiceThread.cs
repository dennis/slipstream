#nullable enable

using DSharpPlus;
using DSharpPlus.Entities;
using Serilog;
using Slipstream.Components.Discord.EventHandler;
using Slipstream.Components.Discord.Events;
using Slipstream.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Slipstream.Components.Discord.Lua
{
    internal class DiscordServiceThread : BaseInstanceThread, IDiscordInstanceThread
    {
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory DiscordEventFactory;
        private readonly string Token;
        private readonly Thread ServiceThead;
        private readonly Dictionary<ulong, DiscordChannel> DiscordChannelIdMap = new Dictionary<ulong, DiscordChannel>();
        private DiscordClient? Client;
        private bool RequestConnect = true;

        public DiscordServiceThread(string instanceId, string token, IEventBusSubscription eventBusSubscription, IEventHandlerController eventHandlerController, IEventBus eventBus, IDiscordEventFactory discordEventFactory, ILogger logger) : base(instanceId, logger)
        {
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            EventBus = eventBus;
            DiscordEventFactory = discordEventFactory;
            Token = token;
            ServiceThead = new Thread(new ThreadStart(Main));
        }

        override protected void Main()
        {
            var eventHandler = EventHandlerController.Get<DiscordEventHandler>();
            eventHandler.OnDiscordCommandSendMessage += (_, e) => OnDiscordCommandSendMessage(e);
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalShutdown += (_, _e) => Stopping = true;

            Logger.Debug($"Starting {nameof(DiscordServiceThread)} {InstanceId}");

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }

                if (RequestConnect)
                {
                    Client = new DiscordClient(new DiscordConfiguration()
                    {
                        Token = Token,
                        TokenType = TokenType.Bot,
                        AutoReconnect = true,
                        EnableCompression = true,
                        LogLevel = LogLevel.Debug,
                    });

                    Client.ConnectAsync().GetAwaiter().GetResult();

                    Client.Ready += Client_Ready;
                    Client.SocketClosed += Client_SocketClosed;
                    Client.MessageCreated += Client_MessageCreated;

                    RequestConnect = false;
                }
            }

            Logger.Debug($"Stopping {nameof(DiscordServiceThread)} {InstanceId}");
        }

        private void OnDiscordCommandSendMessage(DiscordCommandSendMessage e)
        {
            if (Client == null || e.InstanceId != InstanceId)
                return;

            if (!DiscordChannelIdMap.ContainsKey(e.ChannelId))
            {
                var channel = Client.GetChannelAsync(e.ChannelId).GetAwaiter().GetResult();

                DiscordChannelIdMap.Add(e.ChannelId, channel);
            }

            DiscordChannelIdMap[e.ChannelId].SendMessageAsync(e.Message, e.TextToSpeech);
        }

        private Task Client_SocketClosed(DSharpPlus.EventArgs.SocketCloseEventArgs e)
        {
            EventBus.PublishEvent(DiscordEventFactory.CreateDiscordDisconnected(InstanceId));
            return Task.CompletedTask;
        }

        private Task Client_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            EventBus.PublishEvent(DiscordEventFactory.CreateDiscordConnected(InstanceId));

            if (Client == null)
                return Task.CompletedTask;

            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent)
                return Task.CompletedTask;

            EventBus.PublishEvent(DiscordEventFactory.CreateDiscordMessageReceived(
                instanceId: InstanceId,
                fromId: e.Author.Id,
                from: e.Author.Username + "#" + e.Author.Discriminator,
                channelId: e.Channel.Id,
                channel: e.Channel.Name,
                message: e.Message.Content
            ));

            return Task.CompletedTask;
        }
    }
}