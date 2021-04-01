#nullable enable

using DSharpPlus;
using DSharpPlus.Entities;
using Serilog;
using Slipstream.Components.Discord.EventHandler;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slipstream.Components.Discord.Plugins
{
    internal class DiscordPlugin : BasePlugin
    {
        private static readonly DictionaryValidator ConfigurationValidator;

        private bool RequestConnect = true;
        private DiscordClient? Client;
        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory EventFactory;
        private readonly string Token;
        private readonly Dictionary<ulong, DiscordChannel> DiscordChannelIdMap = new Dictionary<ulong, DiscordChannel>();

        static DiscordPlugin()
        {
            ConfigurationValidator = new DictionaryValidator().RequireString("token");
        }

        public DiscordPlugin(IEventHandlerController eventHandlerController, string pluginId, IEventBus eventBus, IDiscordEventFactory eventFactory, Parameters configuration) : base(eventHandlerController, pluginId, "DiscordPlugin", "DiscordPlugin", true)
        {
            ConfigurationValidator.Validate(configuration);

            EventBus = eventBus;
            EventFactory = eventFactory;
            Token = configuration.Extract<string>("token");

            var eventHandler = EventHandlerController.Get<DiscordEventHandler>();

            eventHandler.OnDiscordCommandSendMessage += EventHandler_OnDiscordCommandSendMessage;
        }

        private void EventHandler_OnDiscordCommandSendMessage(object source, Events.DiscordCommandSendMessage e)
        {
            if (Client == null)
                return;

            if (!DiscordChannelIdMap.ContainsKey(e.ChannelId))
            {
                var channel = Client.GetChannelAsync(e.ChannelId).GetAwaiter().GetResult();

                DiscordChannelIdMap.Add(e.ChannelId, channel);
            }

            DiscordChannelIdMap[e.ChannelId].SendMessageAsync(e.Message, e.TextToSpeech);
        }

        public override void Run()
        {
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

            System.Threading.Thread.Sleep(500);
        }

        private Task Client_SocketClosed(DSharpPlus.EventArgs.SocketCloseEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordDisconnected());
            return Task.CompletedTask;
        }

        private Task Client_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateDiscordConnected());

            if (Client == null)
                return Task.CompletedTask;

            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Author.IsCurrent)
                return Task.CompletedTask;

            EventBus.PublishEvent(EventFactory.CreateDiscordMessageReceived(
                channelId: e.Channel.Id,
                channel: e.Channel.Name,
                from: e.Author.Username + "#" + e.Author.Discriminator,
                fromId: e.Author.Id,
                message: e.Message.Content
            ));

            return Task.CompletedTask;
        }
    }
}