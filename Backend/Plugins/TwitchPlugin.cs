using Slipstream.Shared;
using Slipstream.Shared.Events.Twitch;
using Slipstream.Shared.Events.Utility;
using Slipstream.Shared.Events.Setting;
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
        private string? TwitchToken;

        public TwitchPlugin(string id, IEventBus eventBus, TwitchSettings settings) : base(id, "TwitchPlugin", "TwitchPlugin", "TwitchPlugin")
        {
            EventBus = eventBus;
            
            EventHandler.OnSettingTwitchSettings += (s, e) => OnTwitchSettings(e.Event);
            EventHandler.OnTwitchSendMessage += (s, e) =>
            {
                if (Client != null && Client.JoinedChannels.Count > 0)
                {
                    Client.SendMessage(TwitchUsername, e.Event.Message);
                }
            };

            OnTwitchSettings(settings);
        }

        private void OnTwitchSettings(TwitchSettings settings)
        {
            {
                if (TwitchUsername != settings.TwitchUsername || TwitchToken != settings.TwitchToken)
                {
                    TwitchUsername = settings.TwitchUsername;
                    TwitchToken = settings.TwitchToken;

                    Disconnect();
                    Connnect();
                }
            };
        }

        public override void OnDisable()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            Client?.Disconnect();
            Client = null;
        }

        public override void OnEnable()
        {
            Connnect();
        }

        private void Connnect()
        {
            if (!Enabled)
                return;

            if (TwitchUsername == null || TwitchToken == null || TwitchUsername.Length == 0 || TwitchToken.Length == 0)
                return;

            if (Client != null && Client.IsConnected)
                return;

            if(Client == null)
            {
                ConnectionCredentials credentials = new ConnectionCredentials(TwitchUsername, TwitchToken, "ws://irc-ws.chat.twitch.tv:80");
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };

                WebSocketClient customClient = new WebSocketClient(clientOptions);
                Client = new TwitchClient(customClient);
                Client.Initialize(credentials, TwitchUsername);

                Client.OnConnected += OnConnected;
                Client.OnChatCommandReceived += OnChatCommandReceived;
                Client.OnDisconnected += OnDisconnect;
                Client.OnError += Client_OnError;
                Client.OnIncorrectLogin += Client_OnIncorrectLogin;
                Client.OnReconnected += Client_OnReconnected;

                Client.Connect();
            }
        }

        private void Client_OnReconnected(object sender, OnReconnectedEventArgs e)
        {
            EventBus.PublishEvent(new TwitchConnected());
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch connected as {TwitchUsername}" });
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch Error: {e.Exception.Message}" });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { Id = this.Id });
        }

        private void Client_OnError(object sender, OnErrorEventArgs e)
        {
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch Error: {e.Exception.Message}" });
            EventBus.PublishEvent(new Shared.Events.Internal.PluginDisable() { Id = this.Id });
        }

        private void OnDisconnect(object sender, OnDisconnectedEventArgs e)
        {
            EventBus.PublishEvent(new TwitchDisconnected());
            if(Enabled)
                Client?.Reconnect();
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
            EventBus.PublishEvent(new TwitchConnected());
            EventBus.PublishEvent(new WriteToConsole { Message = $"Twitch connected as {TwitchUsername}" });
        }
    }
}
